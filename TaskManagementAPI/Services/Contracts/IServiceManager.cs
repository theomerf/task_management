namespace Services.Contracts
{
    public interface IServiceManager
    {
        IAccountService AccountService { get; }
        IProjectService ProjectService { get; }
        ITaskService TaskService { get; }
        ICommentService CommentService { get; }
        IActivityLogService ActivityLogService { get; }
        INotificationService NotificationService { get; }
    }
}
