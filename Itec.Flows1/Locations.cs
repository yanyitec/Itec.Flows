using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public enum Locations
    {
        /// <summary>
        /// 读写本地数据
        /// </summary>
        Local = 1,
        /// <summary>
        /// 读写Parent的数据
        /// </summary>
        Parent=1<<1,
        /// <summary>
        /// 读写Root的数据
        /// </summary>
        Root = 1<<2,
        /// <summary>
        /// 穿越变量
        /// 谁设定了值，就写到谁那里去
        /// 如果谁都没有设值，就写到Root上去
        /// </summary>
        Overthrough = 1<<3,

        /// <summary>
        /// 外部的Inputs
        /// </summary>
        Inputs = 1 << 4,
        /// <summary>
        /// 配置中的Inputs节
        /// </summary>
        Parameters = 1 << 5,
        /// <summary>
        /// 从Inputs集合中来
        /// </summary>
        Imports = Inputs | Parameters,
        /// <summary>
        /// 会写入到Outputs中
        /// Outputs与Exports的区别请参见Exports的解释
        /// </summary>
        Outputs = 1 << 3,
        /// <summary>
        /// 会写到Exports中
        /// Outputs会作为下一个节点的Inputs
        /// Exports会把数据写入Parent的Locals中
        /// </summary>
        Exports = 1 << 4,
    }
}
