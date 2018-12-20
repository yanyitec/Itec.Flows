using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Itec.Flows
{
    public interface IActivityStateProxy: IActivityState,IDisposable
    {
        /// <summary>
        /// 给当前节点添加一个路由
        /// </summary>
        /// <param name="defination"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void AddRoute(Config defination,string key=null, string value=null );

        void InsertNext(Config defination);
        void AppendNext(Config defination);

        void Deal();

        Task DealAsync();

        IDealer CurrentHandler { get; }


    }
}
