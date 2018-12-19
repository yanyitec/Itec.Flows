using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows.Activities
{
    public class BlockActivity : IActivity
    {
        Queue<ActivityState> _ActiveSubStates; 

        public ActivityState ActivityState { get; internal set; }

        public virtual bool CheckStart(IReadOnlyDictionary<string, string> inputs, IDealer dealer, object context)
        {
            return true;
        }
        public virtual bool CheckOver(IReadOnlyDictionary<string, string> outputs, IDealer dealer, object context)
        {
            return true;
        }

        //IList<ActivityState> RuningStates;

        public IReadOnlyDictionary<string, string> Execute(IState state, IDealer dealer, object context)
        {
            var todos = new Queue<ActivityState>();
            for (int i = 0, c = _ActiveSubStates.Count; i < c; i++)
            {
                var subState = _ActiveSubStates.Dequeue();
                var stage = (ExecuteStages)subState.Entity.ExecuteStage;

                if (stage == ExecuteStages.Finished || stage == ExecuteStages.Aborted) {
                    continue;
                }
                _ActiveSubStates.Enqueue(subState);
                todos.Enqueue(subState);
            }


            while (todos.Count > 0 ) {

                for (int i = 0, c = todos.Count; i < c; i++) {
                    var currState = todos.Dequeue();
                    var stage = currState.Deal(todos, dealer, context);
                    currState.SaveChanges();


                }
            }
            this.ActivityState.SaveChanges();
            return this.ActivityState.ExecuteStage== ExecuteStages.Finished?this.ActivityState.Datas:null;
        }

        RouteResults RouteNexts(ActivityState state,Queue<ActivityState> todos) {
            //var nexts = state.
            return RouteResults.End;
        }
    }
}
