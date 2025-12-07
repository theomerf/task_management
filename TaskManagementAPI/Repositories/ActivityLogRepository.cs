using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Extensions;

namespace Repositories
{
    public class ActivityLogRepository : RepositoryBase<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<ActivityLog> activityLogs, int totalCount)> GetProjectActiviyLogsAsync(long projectId, ActivityLogRequestParameters p, bool trackChanges, long? taskId = null)
        {
            var activityLogsQuery = FindByCondition(al => al.RelatedProjectId == projectId, trackChanges)
                .Include(al => al.PerformedBy)
                .FilterBy(taskId, al => al.RelatedTaskId, FilterOperator.Equal)
                .FilterBy(p.AccountId, al => al.PerformedById, FilterOperator.Equal)
                .FilterByActivityType(p.ActivityType);

            var totalCount = await activityLogsQuery.CountAsync();

            var activityLogs = await activityLogsQuery
                .OrderByDescending(al => al.CreatedAt)
                .ToPaginate(p.PageNumber, p.PageSize)
                .ToListAsync();

            return (activityLogs, totalCount);
        }

        public void CreateActivityLog(ActivityLog activityLog)
        {
            Create(activityLog);
        }
    }
}
