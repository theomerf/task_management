using Entities.Dtos;
using Entities.Models;

namespace Repositories.Contracts
{
    public interface IAuthorizationRepository : IRepositoryBase<Project>
    {
        Task<bool> CanAccessProjectAsync(string accountId, Project project);
        Task<bool> CanManageProjectAsync(string accountId, Project project);
        Task<bool> CanDeleteProjectAsync(string accountId, Project project);
        Task<(bool isAuthorized, long? id)> CanAccessTasksAsync(string accountId, Guid projectId, bool isAdmin);
        Task<(bool isAuthorized, long? id)> CanManageTasksAsync(string accountId, Guid projectId, bool isAdmin);
        Task<(bool isAuthorized, long? id)> CanAccessCommentsAsync(string accountId, Guid projectId, bool isAdmin);
        Task<(bool isAuthorized, long? id)> CanCreateCommentsAsync(string accountId, Guid projectId, bool isAdmin);
        bool CanManageCommentsAsync(string accountId, CommentDtoForManage comment);
        Task<Mention> CanMarkMentionAsReadAsync(string accountId, Guid projectId, Guid taskId, Guid commentId, Guid mentionId);
        Task<(bool isAuthorized, long? id)> CanAccessActivityLogsAsync(string accountId, Guid projectId, bool isAdmin);
    }
}
