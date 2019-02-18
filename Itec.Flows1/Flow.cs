using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class Flow : IFlow
    {
        public IActivityStatePersistent Dao { get; private set; }
        public IActivityStateProxy CreateFlow(string flowId, object inputs,IDealer currentUser)
        {
            throw new NotImplementedException();
        }

        public IActivityStateProxy GetAndLockActivityState(Guid activityId, IDealer user, int retryCount = 0)
        {
            var sql = @"SELECT * FROM ActivityStates 
WHERE RootId IN (SELECT Id FROM ActivityStates WHERE Id=@id)
AND ExecuteStage<5
AND HandlerId=@userId
";

            return null;
            //this.Dao.ListActivityStates()
        }
    }
}
