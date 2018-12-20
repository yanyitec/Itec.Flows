using System;
using System.Collections.Generic;
using System.Text;

namespace Itec.Flows
{
    public class ActivityStateEntity
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// 工作流名称
        /// </summary>
        public string FlowId { get; set; }

        public Guid ParentId { get; set; }

        public Guid RootId { get; set; }

        public Guid PreviousId { get; set; }

        /// <summary>
        /// 活动名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 活动类型Type
        /// </summary>

        public string ActivityType { get; set; }
        /// <summary>
        /// 定义
        /// </summary>

        public string Config { get; set; }
        

        /// <summary>
        /// 执行阶段
        /// </summary>
        public int ExecuteStage { get; set; }

        /// <summary>
        /// 输入
        /// </summary>
        public string Inputs { get; set; }

        /// <summary>
        /// 执行中间状态
        /// </summary>
        public string Variables { get; set; }
        /// <summary>
        /// 输出
        /// </summary>
        public string Outputs { get; set; }
        /// <summary>
        /// 出口数据
        /// </summary>
        public string Exports { get; set; }

        public string Route { get; set; }

        public DateTime ExecuteTime { get; set; }

        public DateTime FinishTime { get; set; }



        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public Guid DealerId { get; set; }
        public string DealerName { get; set; }

        public string DealerInfo { get; set; }
    }
}
