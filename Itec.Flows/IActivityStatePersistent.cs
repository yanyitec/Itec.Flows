using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Itec.Flows
{
    public interface IActivityStatePersistent
    {
        /// <summary>
        /// 根据Id获取工作流活动状态
        /// </summary>
        /// <param name="activityStateId"></param>
        /// <returns></returns>
        ActivityStateEntity GetActivityStateById(Guid activityStateId);

        /// <summary>
        /// 根据Id异步获取工作流活动状态
        /// </summary>
        /// <param name="activityStateId"></param>
        /// <returns></returns>
        Task<ActivityStateEntity> GetActivityStateByIdAsync(Guid activityStateId);
        /// <summary>
        /// 试图给工作流根活动状态加锁
        /// </summary>
        /// <param name="activityStateId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        ActivityStateEntity TryLockActivityState(Guid activityStateId,IDealer user);
        /// <summary>
        /// 异步地试图给工作流根活动状态加锁
        /// </summary>
        /// <param name="activityStateId"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<ActivityStateEntity> TryLockActivityStateAsync(Guid activityStateId,IDealer dealer);
        /// <summary>
        /// 给工作流活动状态解锁
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dealer"></param>
        void UnlockActivityState(Guid id, IDealer dealer);
        /// <summary>
        /// 异步地给工作流活动状态解锁
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dealer"></param>
        /// <returns></returns>
        Task UnlockActivityStateAsync(Guid id, IDealer dealer);
        /// <summary>
        /// 列举所有根为指定ID的工作流活动状态
        /// </summary>
        /// <param name="rootActivityStateId"></param>
        /// <param name="isActive">是否是活跃的状态</param>
        /// <returns></returns>
        List<ActivityStateEntity> ListActivityStates(Guid rootActivityStateId,bool isActive=true);
        /// <summary>
        /// (异步)列举所有根为指定ID的工作流活动状态
        /// </summary>
        /// <param name="rootActivityStateId"></param>
        /// <param name="isActive">是否是活跃的状态</param>
        /// <returns></returns>
        Task<List<ActivityStateEntity>> ListActivityStatesAsync(Guid rootActivityStateId, bool isActive = true);
        /// <summary>
        /// 保存所有变更
        /// </summary>
        /// <param name="states"></param>
        void SaveStatesChanges(IList<ActivityStateEntity> states);
        /// <summary>
        /// (异步)保存所有变更
        /// </summary>
        /// <param name="states"></param>
        Task SaveStatesChangesAsync(IList<ActivityStateEntity> states);
        /// <summary>
        /// 创建新的活动状态
        /// </summary>
        /// <param name="entity"></param>
        void CreateActivityState(ActivityStateEntity entity);
        /// <summary>
        /// (异步)创建新的活动状态
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="executeStage"></param>
        /// <returns></returns>
        Task CreateActivityStateAsync(ActivityStateEntity entity, ExecuteStages executeStage);
    }
}
