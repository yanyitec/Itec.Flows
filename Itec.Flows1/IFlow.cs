using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public interface IFlow
    {
        /// <summary>
        /// 创建工作流
        /// </summary>
        /// <param name="flowId"></param>
        /// <param name="inputs"></param>
        /// <returns></returns>
        IActivityStateProxy CreateFlow(string flowId, object inputs,IDealer currentUser);


        IActivityStateProxy GetAndLockActivityState(Guid activityId, IDealer user,int retryCount=0);
    }
}
