namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository Project { get; }
        IAuthorizationRepository Authorization { get; }
        void Save();
        Task SaveAsync();
    }
}
