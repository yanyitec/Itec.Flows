using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    [Flags]
    public enum Scopes
    {
        /// <summary>
        /// 内部变量,默认
        /// </summary>
        Variables=1,
        /// <summary>
        /// 外部的Inputs
        /// </summary>
        Inputs =1<<1,
        /// <summary>
        /// 配置中的Inputs节
        /// </summary>
        Parameters=1<<2,
        /// <summary>
        /// 从Inputs集合中来
        /// </summary>
        In = Inputs | Parameters,
        /// <summary>
        /// 会写入到Outputs中
        /// Outputs与Exports的区别请参见Exports的解释
        /// </summary>
        Outputs = 1<<3,
        /// <summary>
        /// 会写到Exports中
        /// Outputs会作为下一个节点的Inputs
        /// Exports会把数据写入Parent的Locals中
        /// </summary>
        Exports = 1<<4,
        /// <summary>
        /// Out会同时写入Exports与Outputs集合
        /// </summary>
        Out= Outputs | Exports,
        /// <summary>
        /// 该变量来自root
        /// </summary>
        Root =1<<5,
        Parent = 1<<6,
        Overall = 1<<7

        
    }
}
