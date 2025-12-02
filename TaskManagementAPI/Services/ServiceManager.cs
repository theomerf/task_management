using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;
        private readonly Lazy<IProjectService> _projectService;

        public ServiceManager(IServiceProvider provider)
        {
            _accountService = new Lazy<IAccountService>(() => provider.GetRequiredService<IAccountService>());
            _projectService = new Lazy<IProjectService>(() => provider.GetRequiredService<IProjectService>());  
        }

        public IAccountService AccountService => _accountService.Value;
        public IProjectService ProjectService => _projectService.Value;
    }
}
