using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Itec.Flows
{
    public class ActivityStateSqlitePersistent:IActivityStatePersistent
    {
        public ActivityStateSqlitePersistent(string dbName) {

        }

        

        public void CreateActivityState(ActivityStateEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task CreateActivityStateAsync(ActivityStateEntity entity, ExecuteStages executeStage)
        {
            throw new NotImplementedException();
        }

        public ActivityStateEntity GetActivityStateById(Guid activityStateId)
        {
            throw new NotImplementedException();
        }

        public Task<ActivityStateEntity> GetActivityStateByIdAsync(Guid activityStateId)
        {
            throw new NotImplementedException();
        }

        public List<ActivityStateEntity> ListActivityStates(Guid rootActivityStateId, bool isActive = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<ActivityStateEntity>> ListActivityStatesAsync(Guid rootActivityStateId, bool isActive = true)
        {
            throw new NotImplementedException();
        }

        public void SaveStatesChanges(IList<ActivityStateEntity> states)
        {
            throw new NotImplementedException();
        }

        public Task SaveStatesChangesAsync(IList<ActivityStateEntity> states)
        {
            throw new NotImplementedException();
        }

        public ActivityStateEntity TryLockActivityState(Guid activityStateId, IDealer user)
        {
            throw new NotImplementedException();
        }

        public Task<ActivityStateEntity> TryLockActivityStateAsync(Guid activityStateId, IDealer dealer)
        {
            throw new NotImplementedException();
        }

        public void UnlockActivityState(Guid id, IDealer dealer)
        {
            throw new NotImplementedException();
        }

        public Task UnlockActivityStateAsync(Guid id, IDealer dealer)
        {
            throw new NotImplementedException();
        }
    }
}
