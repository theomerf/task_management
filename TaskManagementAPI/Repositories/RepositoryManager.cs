using Microsoft.Extensions.DependencyInjection;
using Repositories.Contracts;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly RepositoryContext _context;
        private readonly Lazy<IProjectRepository> _projectRepository;
        private readonly Lazy<IAuthorizationRepository> _authorizationRepository;
        private readonly Lazy<ITaskRepository> _taskRepository;
        private readonly Lazy<ICommentRepository> _commentRepository;
        private readonly Lazy<IActivityLogRepository> _activityLogRepository;
        private readonly Lazy<INotificationRepository> _notificationRepository;

        public RepositoryManager(RepositoryContext context, IServiceProvider provider)
        {
            _context = context;
            _projectRepository = new Lazy<IProjectRepository>(() => provider.GetRequiredService<IProjectRepository>());
            _authorizationRepository = new Lazy<IAuthorizationRepository>(() => provider.GetRequiredService<IAuthorizationRepository>());
            _taskRepository = new Lazy<ITaskRepository>(() => provider.GetRequiredService<ITaskRepository>());
            _commentRepository = new Lazy<ICommentRepository>(() => provider.GetRequiredService<ICommentRepository>());
            _activityLogRepository = new Lazy<IActivityLogRepository>(() => provider.GetRequiredService<IActivityLogRepository>());
            _notificationRepository = new Lazy<INotificationRepository>(() => provider.GetRequiredService<INotificationRepository>());
        }

        public IProjectRepository Project => _projectRepository.Value;
        public IAuthorizationRepository Authorization => _authorizationRepository.Value;
        public ITaskRepository Task => _taskRepository.Value;
        public ICommentRepository Comment => _commentRepository.Value;
        public IActivityLogRepository ActivityLog => _activityLogRepository.Value;
        public INotificationRepository Notification => _notificationRepository.Value;

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
