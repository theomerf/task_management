namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository Project { get; }
        IAuthorizationRepository Authorization { get; }
        ITaskRepository Task { get; }
        ICommentRepository Comment { get; }
        IActivityLogRepository ActivityLog { get; }
        INotificationRepository Notification { get; }
        void Save();
        Task SaveAsync();
    }
}
