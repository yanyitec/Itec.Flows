using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itec.Flows
{
    public class ActivityState :CascadeReadOnlyState
    {
        //public readonly static IReadOnlyDictionary<string, string> NoResults = new Dictionary<string, string>();
        public ActivityState(ActivityState parent,ActivityStateEntity entity):base(()=> entity.Locals==null?null:JObject.Parse(entity.Locals),parent) {
            this.Entity = entity;
            this.Parent = parent;
            this.Root = parent == null ? this : parent.Root;
        }

        public ActivityState(string flowId, Defination defination, JObject input,IDealer dealer=null)
            :this(null,flowId,defination,input, Guid.Empty ,dealer)
        {
            
        }

        public ActivityState(ActivityState parent, Defination defination, JObject input,Guid previousId, IDealer dealer=null)
            : this(parent, null, defination, input,previousId,dealer)
        {

        }

        ActivityState(ActivityState parent,string flowId, Defination defination,JObject input,Guid previousId,IDealer dealer=null)
            : base(parent)
        {
            this.Entity = new ActivityStateEntity();
            this.Entity.Id = Guid.NewGuid();
            this.Entity.PreviousId = previousId;
            if (parent == null)
            {
                this.Entity.FlowId = this.FlowId = flowId;
                this.Root = this;
                this.Entity.RootId = this.Entity.Id;
                this.Entity.ParentId = Guid.Empty;
            }
            else {
                this.Entity.FlowId = this.FlowId = parent.FlowId;
                this.Root = parent.Root;
                this.Parent = parent;
                this.Entity.RootId = this.Root.Id;
                this.Entity.ParentId = parent.Id;
            }
            this._Inputs = new ReadOnlyState(input);
            this.Entity.Inputs = this._Inputs.ToJSON();
            if (parent != null) {
                this.Internals = (parent.Internals.DeepClone() as JObject) ?? new JObject();
            } else {
                this.Internals = new JObject();
            }
            

            this.Dealer = dealer ?? MakeDealer();
            if (this.Dealer != null) {
                this.Entity.DealerId = this.Dealer.Id;
                this.Entity.DealerName = this.Dealer.Name;
                this.Entity.DealerInfo = this.Dealer.ToJSON();
            }
        }

        IDealer MakeDealer() {
            return null;
        }
        IDealer _Dealer;
        bool _DealerChanged;

        /// <summary>
        /// 当前处理人
        /// </summary>
        public IDealer Dealer {
            get {
                return _Dealer;
            }
            set {
                _Dealer = value;
                _DealerChanged = true;
            }
        }
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get { return this.Entity.Id; } }

        /// <summary>
        /// 工作流Id
        /// </summary>
        public string FlowId { get;private set; }
        /// <summary>
        /// 持久化用的实体
        /// </summary>
        public ActivityStateEntity Entity { get;private set; }

        /// <summary>
        /// 持久化对象
        /// 数据库访问对象
        /// DAO,Repository
        /// </summary>
        public IActivityStatePersistent Persistent { get; private set; }

        public ActivityState Parent { get; private set; }
        public ActivityState Root { get; private set; }

        public string Path { get; private set; }

        

        public Defination Defination { get; set; }

        bool _DefinationChanged;

        
        ReadOnlyState _Inputs;
        /// <summary>
        /// 状态数据，在Deal过程中会使用
        /// </summary>
        public IReadOnlyState Inputs
        {
            get
            {
                if (_Inputs == null)
                {
                    _Inputs = new ReadOnlyState(Entity.Inputs);
                }
                return _Inputs;
            }
        }

        State _Locals;
        /// <summary>
        /// 状态数据，在Deal过程中会使用
        /// </summary>
        public IState Locals
        {
            get
            {
                if (_Locals == null)
                {
                    _Locals = new State(this.Entity.Locals);
                }
                return _Locals;
            }
        }



        State _Outputs;
        /// <summary>
        /// 状态数据，在Deal过程中会使用
        /// </summary>
        public IState Outputs
        {
            get
            {
                if (_Outputs == null)
                {
                    _Outputs = new State(this.Entity.Outputs);
                }
                return _Outputs;
            }
        }

        public string Route {
            get { return this.Entity.Route; }
            set { this.Entity.Route = value; }
        }

        public IActivity Activity { get; private set; }

        

        public ExecuteStages ExecuteStage {
            get {
                return (ExecuteStages)this.Entity.ExecuteStage;
            }
            set {
                lock (this) {
                    this.Entity.ExecuteStage = (int)value;
                }
                
            }
        }

        
        
        public  void SaveChanges() {
            var states = new List<ActivityStateEntity>();
            this.SaveChanges(states);
        }

        ActivityStateEntity CheckChanges() {
            var entity = new ActivityStateEntity() {
                Id = this.Entity.Id
            };
            var hasChanges = false;
            if (this.HasChanges)
            {
                entity.Locals = this.Internals.ToString();
                this.HasChanges = false;
                hasChanges = true;
            }
            
            if (this._Outputs!=null && this._Outputs.HasChanges)
            {
                entity.Outputs = this._Outputs.ToString();
                this._Outputs.HasChanges = false;
                hasChanges = true;
            }
            return hasChanges?entity:null;
        }

        void SaveChanges(IList<ActivityStateEntity> states) {
            
            var entity = CheckChanges();
            if (entity != null) states.Add(entity);
            if (this.Parent == null)
            {
                this.Persistent.SaveStatesChanges(states);
            }
            else {
                this.Parent.SaveChanges(states);
            }
        }

        void CombineState(IReadOnlyState append) {
            if (append != null && append.HasChanges)
            {
                var state = append as ReadOnlyState;
                
                foreach (var pair in state.Internals)
                {
                    Internals[pair.Key] = pair.Value;
                }
            }
        }

        



        
        public ExecuteStages Deal(Queue<ActivityState> todos, IDealer dealer, object context=null) {
            if (!this.Activity.CheckStart(this.Inputs,new ReadOnlyState(this.Defination.Parameters),this, dealer, context)) return ExecuteStages.Denied;
            if (this.ExecuteStage == ExecuteStages.Creating) {

                this.ExecuteStage = this.InsertActivity(dealer,context);
            }
            if (this.ExecuteStage == ExecuteStages.Created) {
                this.MergeStates();
            }

            if (this.ExecuteStage == ExecuteStages.Executing || this.ExecuteStage== ExecuteStages.Created) {
                
                this.ExecuteStage = this.ExecuteActivity(dealer,null,context);
            }

            if (this.ExecuteStage == ExecuteStages.Executed) {
                if (this.Activity.CheckOver(this.Outputs, dealer, context)) {
                    this.ExecuteStage = ExecuteStages.Routing;
                }
            }
            if (this.ExecuteStage == ExecuteStages.Routing) {
                if (todos != null)
                    this.ExecuteStage = RouteNexts(todos);
                else
                    this.ExecuteStage = ExecuteStages.Finished;
            }
            this.SaveChanges();
            return this.ExecuteStage;
        }

        void MergeStates() {
            var internals = (this.Parent as CascadeReadOnlyState)?.MergeTo()??new JObject();



            var pars = this.Defination.Parameters;
            //合并pars到当前私有变量
            if (pars != null)
            {
                foreach (var pair in pars)
                {
                    internals[pair.Key] = pair.Value;
                }
            }

            //合并Input到私有数据中
            foreach (var pair in _Inputs.Internals)
            {
                internals[pair.Key] = pair.Value;
            }
            if (this._Locals != null)
            {
                this.Internals = internals;
            }
            else {
                this._Locals = new State(internals);
            }
            //this.Entity.Locals = this.ToJSON();
        }

        /// <summary>
        /// 在数据库中新添一条ActivityState记录
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="inputs"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        ExecuteStages InsertActivity(IDealer dealer,  object context)
        {
            var entity = this.Entity;
            entity.ExecuteStage = (int)ExecuteStages.Created;
            
            this.Persistent.CreateActivityState(entity);
            return ExecuteStages.Created;
        }

        /// <summary>
        /// 执行Activity.Execute方法
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="state"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        ExecuteStages ExecuteActivity(IDealer dealer, IState state, object context) {
            var isDone = this.Activity.Execute(state,dealer, context);
            if (!isDone) return ExecuteStages.Executing;

            if (this.Defination != null && this.Parent!=null) {
                var exports = this.Defination.Exports;
                if (exports != null) {
                    var parentDatas = this.Parent.Internals;
                    var localDatas = this.Internals;
                    foreach (var key in exports) {
                        parentDatas[key] = localDatas[key];
                    }
                }
            }

            return ExecuteStages.Executed;
        }

        #region route
        /// <summary>
        /// 查找下一个节点
        /// </summary>
        /// <returns></returns>
        ExecuteStages RouteNexts(Queue<ActivityState> todos)
        {
            
            var nexts = this.Defination.Nexts;
            if (nexts == null) return this.Parent==null?ExecuteStages.End: ExecuteStages.Routing; 
            if ( nexts.Type == JTokenType.Null || nexts.Type == JTokenType.Undefined) return ExecuteStages.End;

            if (this.Parent == null) throw new DefinationException("根活动不能定义Nexts",null,this.FlowId,this.Defination);
            //string nextActivityName = null;
            if (nexts.Type == JTokenType.String)
            {
                return this.RouteNextsByString(todos, nexts);
            }
            else if (nexts.Type == JTokenType.Array)
            {
                return RouteNextsByArray(todos, nexts);

            }
            else if (nexts.Type == JTokenType.Object) {
                return RouteNextsByDictionary(todos,nexts);
            }
            throw new RouteException("不正确的Route配置,只能是null,undefined,string,数组或词典", null, this, nexts);


        }
        static readonly string EndToken = "%END";
        ExecuteStages RouteNextsByString(Queue<ActivityState> todos,JToken nexts) {
            var name = nexts.ToString();
            if (name == EndToken)
                return ExecuteStages.End;
            else
            {
                if (AddTodos(todos, name))
                    return ExecuteStages.Finished;
                else
                    return ExecuteStages.Routing;
            }
        }
        ExecuteStages RouteNextsByArray(Queue<ActivityState> todos, JToken nexts)
        {
            var nxts = nexts as JArray;
            bool routed = false;
            for (int i = 0, j = nxts.Count; i < j; i++)
            {
                var nextToken = nxts[i];
                if (nextToken.Type == JTokenType.String)
                {
                    var name = nexts.ToString();
                    if (!this.AddTodos(todos, name)) throw new RouteException("未能添加下一个执行节点", name, this, nexts);
                    else routed = true;
                }
                else if (nextToken.Type == JTokenType.Object)
                {
                    var route = nextToken as JObject;
                    var routeName = route["Name"];
                    var routeValue = route["Value"];
                    if (routeName == null || routeValue == null)
                    {
                        throw new RouteException("不正确的Route配置,缺乏Name或Value", route.ToString(), this, nexts);
                    }
                    var name = routeName.ToString();
                    if (this.Route == name)
                    {
                        if (this.AddTodos(todos, routeValue.ToString()))
                        {
                            routed = true;
                        }
                        else
                        {
                            throw new RouteException("未能找到指定的节点", route.ToString(), this, nexts);
                        }
                    }
                }
                else
                {
                    throw new RouteException("不正确的Route配置,数组中只能出现字符串或词典", null, this, nexts);
                }
            }
            if (routed) return ExecuteStages.Finished;
            return ExecuteStages.Routing;

        }
        ExecuteStages RouteNextsByDictionary(Queue<ActivityState> todos, JToken nexts)
        {
            var nxts = nexts as JObject;
            var routed = false;
            foreach (var pair in nxts) {
                var name = pair.Key;
                var val = pair.Value;
                if (this.Route == name) {
                    if (val.Type == JTokenType.Null || val.Type == JTokenType.Undefined) return ExecuteStages.End;
                    if(val.Type!= JTokenType.String)
                        throw new RouteException("不正确的Route配置,只能出现字符串词典", null, this, nexts);
                    var valstr = val.ToString();
                    if (valstr == EndToken) return ExecuteStages.End;
                    if (AddTodos(todos, valstr)) routed = true;
                }
            }
            return routed ? ExecuteStages.Finished : ExecuteStages.Routing;
        }
        bool AddTodos(Queue<ActivityState> todos,string activityName) {
            
            if (this.Parent.Defination.Activities == null) throw new DefinationException("Route之前必须配置Defination",this.Parent.Path,this.Parent.FlowId,this.Parent.Defination);
            var nextDef = this.Defination.Activities.FirstOrDefault(a => a.Name == activityName);
            if(nextDef==null) return false;
            todos.Enqueue(new ActivityState(this.Parent,nextDef,this._Outputs,this.Id,null));
            return true;
        }
        #endregion

    }
}
