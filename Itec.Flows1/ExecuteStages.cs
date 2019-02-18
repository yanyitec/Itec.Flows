using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public enum ExecuteStages
    {
        /// <summary>
        /// 创建中
        /// 默认阶段，只存在于todo中，还未写入数据库
        /// </summary>
        Creating = 0,
        /// <summary>
        /// 已创建
        /// 已经写入数据库，准备执行
        /// </summary>
        Created =1,
        /// <summary>
        /// 执行中
        /// Activity.Execute调用过一次，但未返回执行结果。要求再次调用
        /// 如果再次调用还未返回结果，将会停留在这个阶段
        /// </summary>
        Executing =2,
        /// <summary>
        /// 已执行
        /// 已经执行Activity.Execute，并返回结果。等待结束检查
        /// 如果结束检查不通过，将会停留在这个阶段
        /// </summary>
        Executed =3 ,
        /// <summary>
        /// 路由中
        /// 配置了Routes节，但根据当前状态情况，未能找到后继的Activity
        /// 等待状态发生变化
        /// </summary>
        Routing =4,
        /// <summary>
        /// 已结束
        /// 执行完成，将从执行队列中移除
        /// </summary>
        Finished=5,
        /// <summary>
        /// 已结束，且是结束节点
        /// 定义中未配置Routes节(意味着是结束节点)
        /// </summary>
        End =6,
        /// <summary>
        /// 已终止
        /// 发生错误时会返回该值
        /// </summary>
        Aborted=7,
        /// <summary>
        /// 入口检查未通过，什么都没做
        /// </summary>
        Denied =8
    }
}
