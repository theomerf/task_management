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

        public RepositoryManager(RepositoryContext context, IServiceProvider provider)
        {
            _context = context;
            _projectRepository = new Lazy<IProjectRepository>(() => provider.GetRequiredService<IProjectRepository>());
            _authorizationRepository = new Lazy<IAuthorizationRepository>(() => provider.GetRequiredService<IAuthorizationRepository>());
            _taskRepository = new Lazy<ITaskRepository>(() => provider.GetRequiredService<ITaskRepository>());
        }

        public IProjectRepository Project => _projectRepository.Value;
        public IAuthorizationRepository Authorization => _authorizationRepository.Value;
        public ITaskRepository Task => _taskRepository.Value;

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
