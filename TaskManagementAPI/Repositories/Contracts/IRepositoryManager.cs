namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IProjectRepository Project { get; }
        IAuthorizationRepository Authorization { get; }
        ITaskRepository Task { get; }
        void Save();
        Task SaveAsync();
    }
}
