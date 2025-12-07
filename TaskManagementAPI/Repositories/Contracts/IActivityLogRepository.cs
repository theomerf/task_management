using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts
{
    public interface IActivityLogRepository : IRepositoryBase<ActivityLog>
    {
        Task<(IEnumerable<ActivityLog> activityLogs, int totalCount)>GetProjectActiviyLogsAsync(long projectId, ActivityLogRequestParameters p, bool trackChanges, long? taskId = null);
        void CreateActivityLog(ActivityLog activityLog);
    }
}
