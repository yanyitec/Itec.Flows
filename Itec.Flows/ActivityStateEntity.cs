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

        public string Defination { get; set; }
        

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
        public string Datas { get; set; }
        /// <summary>
        /// 输出
        /// </summary>
        public string Outputs { get; set; }

        public DateTime ExecuteTime { get; set; }

        public DateTime FinishTime { get; set; }



        public DateTime CreateTime { get; set; }

        public string CreatorId { get; set; }
        public string CreatorName { get; set; }

        public string CreatorInfo { get; set; }
    }
}
