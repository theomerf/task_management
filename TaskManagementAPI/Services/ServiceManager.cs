using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IProjectService> _projectService;
        private readonly Lazy<ITaskService> _taskService;
        private readonly Lazy<ICommentService> _commentService;
        private readonly Lazy<IActivityLogService> _activityLogService;
        private readonly Lazy<INotificationService> _notificationService;

        public ServiceManager(IServiceProvider provider)
        {
            _accountService = new Lazy<IAccountService>(() => provider.GetRequiredService<IAccountService>());
            _projectService = new Lazy<IProjectService>(() => provider.GetRequiredService<IProjectService>());  
            _taskService = new Lazy<ITaskService>(() => provider.GetRequiredService<ITaskService>());
            _commentService = new Lazy<ICommentService>(() => provider.GetRequiredService<ICommentService>());
            _activityLogService = new Lazy<IActivityLogService>(() => provider.GetRequiredService<IActivityLogService>());
            _notificationService = new Lazy<INotificationService>(() => provider.GetRequiredService<INotificationService>());
        }

        public IAccountService AccountService => _accountService.Value;
        public IProjectService ProjectService => _projectService.Value;
        public ITaskService TaskService => _taskService.Value;
        public ICommentService CommentService => _commentService.Value;
        public IActivityLogService ActivityLogService => _activityLogService.Value;
        public INotificationService NotificationService => _notificationService.Value;
    }
}
