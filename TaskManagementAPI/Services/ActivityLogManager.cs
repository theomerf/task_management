using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class ActivityLogManager : IActivityLogService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public ActivityLogManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task<(IEnumerable<ActivityLogDto> pagedActivityLogs, MetaData metaData)> GetProjectActiviyLogsAsync(Guid projectId, ActivityLogRequestParameters p, string accountId, bool isAdmin)
        {
            var canAccess = await _manager.Authorization.CanAccessActivityLogsAsync(accountId, projectId, isAdmin);

            if (!canAccess.isAuthorized)
                throw new ForbiddenException("Bu projenin etkinlik günlüklerine erişim izniniz yok.");

            long? taskSequence = null;

            if (p.TaskId != null)
            {
                taskSequence = await _manager.Task.GetTaskIdAsync(canAccess.id!.Value, p.TaskId.Value);
            }

            var activityLogs = await _manager.ActivityLog.GetProjectActiviyLogsAsync(canAccess.id!.Value, p, false, taskSequence);
            var activityLogsDto = _mapper.Map<IEnumerable<ActivityLogDto>>(activityLogs.activityLogs);

            var pagedActivityLogs = PagedList<ActivityLogDto>.ToPagedList(activityLogsDto, p.PageNumber, p.PageSize, activityLogs.totalCount);

            return (pagedActivityLogs, pagedActivityLogs.MetaData);
        }
    }
}
