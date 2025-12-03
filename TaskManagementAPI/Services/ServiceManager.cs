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

        public ServiceManager(IServiceProvider provider)
        {
            _accountService = new Lazy<IAccountService>(() => provider.GetRequiredService<IAccountService>());
            _projectService = new Lazy<IProjectService>(() => provider.GetRequiredService<IProjectService>());  
            _taskService = new Lazy<ITaskService>(() => provider.GetRequiredService<ITaskService>());
        }

        public IAccountService AccountService => _accountService.Value;
        public IProjectService ProjectService => _projectService.Value;
        public ITaskService TaskService => _taskService.Value;
    }
}
