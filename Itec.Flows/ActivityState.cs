using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Itec.Flows
{
    public class ActivityState 
    {
        public readonly static IReadOnlyDictionary<string, string> NoResults = new Dictionary<string, string>();
        public ActivityState(ActivityState parent,ActivityStateEntity entity) {
            this.Entity = entity;
            this.Parent = parent;
        }

        public ActivityState(ActivityState parent,ActivityDefination defination,IReadOnlyDictionary<string,string> prevOutputs)
        {
            this.Parent = parent;
            this.Entity = new ActivityStateEntity();
            var inputs = new Dictionary<string, string>();
            var data = new Dictionary<string, string>();
            if (prevOutputs != NoResults && prevOutputs!=null)
            {
                foreach (var pair in prevOutputs)
                {
                    inputs.Add(pair.Key, pair.Value);
                    data.Add(pair.Key, pair.Value);
                }
            }
            this._Inputs = inputs;
            this._Datas = data;
            this._DataChanged = true;
        }
        public string FlowId { get;private set; }
        /// <summary>
        /// 与持久化
        /// </summary>
        public ActivityStateEntity Entity { get;private set; }

        /// <summary>
        /// 持久化
        /// 数据库访问对象
        /// DAO,Repository
        /// </summary>
        public IActivityStateDao Dao { get; private set; }

        public ActivityState Parent { get; private set; }
        public ActivityState Root { get; private set; }

        public string Path { get; private set; }

        public ActivityDefination FindActivityDefination(string name) {
            if (this.Defination.Activities == null) return null;
            return this.Defination.Activities.FirstOrDefault(a=>a.Name==name);
        }

        public ActivityDefination Defination { get; set; }

        bool _DataChanged;
        bool _DefinationChanged;

        Dictionary<string, string> _Datas;
        /// <summary>
        /// 状态数据，在Deal过程中会使用
        /// </summary>
        protected internal Dictionary<string, string> Datas {
            get {
                if (_Datas == null) {
                    lock (this) {
                        if (_Datas == null) {
                            _Datas = Utils.FromJson(Entity.Datas,typeof(Dictionary<string, string>)) as Dictionary<string,string>;
                        }
                    }
                }
                return _Datas;
            }
        }
        Dictionary<string, string> _Inputs;
        /// <summary>
        /// 状态数据，在Deal过程中会使用
        /// </summary>
        public IReadOnlyDictionary<string, string> Inputs
        {
            get
            {
                if (_Inputs == null)
                {
                    lock (this)
                    {
                        if (_Inputs == null)
                        {
                            _Inputs = Utils.FromJson(Entity.Inputs, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
                        }
                    }
                }
                return _Inputs;
            }
        }

        bool _OutputChanged;

        IReadOnlyDictionary<string, string> _Outputs;
        /// <summary>
        /// 状态数据，在Deal过程中会使用
        /// </summary>
        public IReadOnlyDictionary<string, string> Outputs
        {
            get
            {
                if (_Outputs == null)
                {
                    lock (this)
                    {
                        if (_Outputs == null)
                        {
                            _Outputs = Utils.FromJson(Entity.Outputs, typeof(Dictionary<string, string>)) as Dictionary<string, string>;
                        }
                    }
                }
                return _Outputs as IReadOnlyDictionary<string,string>;
            }
            set {
                _Outputs = value ?? NoResults;
                _OutputChanged = true;
            }
        }

        public IActivity Activity { get; set; }

        

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

        public string this[string key] { get { return null; } set { } }
        
        public  void SaveChanges() {
            var states = new List<ActivityStateEntity>();
            this.SaveChanges(states);
        }

        ActivityStateEntity CheckChanges() {
            var entity = new ActivityStateEntity() {
                Id = this.Entity.Id
            };
            var hasChanges = false;
            if (this._DataChanged)
            {
                entity.Datas = Newtonsoft.Json.JsonConvert.SerializeObject(this._Datas);
                this._DataChanged = false;
                hasChanges = true;
            }
            
            if (this._OutputChanged)
            {
                entity.Outputs = Newtonsoft.Json.JsonConvert.SerializeObject(this._Outputs);
                this._DataChanged = false;
                hasChanges = true;
            }
            return hasChanges?entity:null;
        }

        void SaveChanges(IList<ActivityStateEntity> states) {
            
            var entity = CheckChanges();
            if (entity != null) states.Add(entity);
            if (this.Parent == null)
            {
                this.Dao.SaveStatesChanges(states);
            }
            else {
                this.Parent.SaveChanges(states);
            }
        }

        void CombineExecuteResults(IReadOnlyDictionary<string, string> results) {
            var datas = this.Datas;
            foreach (var pair in results)
            {
                if (datas.ContainsKey(pair.Key)) datas[pair.Key] = pair.Value;
                datas.Add(pair.Key, pair.Value);
            }
            this._DataChanged = true;
        }



        
        public ExecuteStages Deal(Queue<ActivityState> todos, IDealer dealer, object context=null, IReadOnlyDictionary<string, string> inputs=null) {
            if (!this.Activity.CheckStart(inputs, dealer, context)) return ExecuteStages.Denied;
            if (this.ExecuteStage == ExecuteStages.Creating) {
                this.ExecuteStage = this.InsertActivity(dealer,inputs,context);
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

        /// <summary>
        /// 在数据库中新添一条ActivityState记录
        /// </summary>
        /// <param name="dealer"></param>
        /// <param name="inputs"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        ExecuteStages InsertActivity(IDealer dealer, IReadOnlyDictionary<string, string> inputs, object context)
        {
            var entity = this.Entity;
            entity.ExecuteStage = (int)ExecuteStages.Created;
            var inputInfo = Newtonsoft.Json.JsonConvert.SerializeObject(this.Inputs);
            entity.Inputs = entity.Datas = inputInfo;
            this._DataChanged = false;
            this.Dao.CreateActivityState(entity);
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
            var results = this.Activity.Execute(state,dealer, context);
            if (results == null) return ExecuteStages.Executing;
            
            this._Outputs = results;
            this._OutputChanged = true;
            if (results != NoResults && this.Parent!=null)
            {
                this.Parent.CombineExecuteResults(results);
            }

            return ExecuteStages.Executed;
        }
        /// <summary>
        /// 查找下一个节点
        /// </summary>
        /// <returns></returns>
        ExecuteStages RouteNexts(Queue<ActivityState> todos)
        {
            var routes = this.Defination.Routes;
            if (routes == null || routes.Count == 0) return ExecuteStages.End;
            bool routed = false;
            foreach (var route in routes) {
                string name = null;
                if (route.Key != null) {
                    var stateValue = this[route.Key];
                    if (stateValue == route.Value) name = route.Name;
                }else if (route.Key == null || route.Value == null) {
                    name = route.Name;
                    
                }
                if (name == null) throw new DefinationException("Route.Name不能为空，必须指定下一个的名字", this.Path, this.FlowId, this.Defination);
                var nextDef = this.Parent.FindActivityDefination(name);
                if (nextDef == null)
                    throw new DefinationException("Route中指定了name=" + name + "，但在Activities中，并未定义具有该名称的节点", this.Path, this.FlowId, this.Parent.Defination);
                var nextState = new ActivityState(this.Parent, nextDef,this.Outputs);
                //加入到执行队列中
                todos.Enqueue(nextState);
                routed = true;
            }
            if (routed) return ExecuteStages.Finished;
            return ExecuteStages.Routing;
        }


    }
}
