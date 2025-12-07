using Entities.Dtos;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IActivityLogService
    {
        Task<(IEnumerable<ActivityLogDto> pagedActivityLogs, MetaData metaData)> GetProjectActiviyLogsAsync(Guid projectId, ActivityLogRequestParameters p, string accountId, bool isAdmin);
    }
}
