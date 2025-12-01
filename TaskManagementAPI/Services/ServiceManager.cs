using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Services.Contracts;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAccountService> _accountService;

        public ServiceManager(IServiceProvider provider)
        {
            _accountService = new Lazy<IAccountService>(() => provider.GetRequiredService<IAccountService>());
        }

        public IAccountService AccountService => _accountService.Value;
    }
}
