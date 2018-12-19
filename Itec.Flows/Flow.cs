using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class Flow : IFlow
    {
        public IActivityStateDao Dao { get; private set; }
        public IActivityState CreateFlow(string flowId, IReadOnlyDictionary<string, string> inputs)
        {
            throw new NotImplementedException();
        }

        public IActivityState GetAndLockActivityState(Guid id, IDealer user)
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
