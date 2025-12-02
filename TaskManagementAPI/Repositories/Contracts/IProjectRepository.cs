using Entities.Dtos;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts
{
    public interface IProjectRepository : IRepositoryBase<Project>
    {
        Task<(IEnumerable<ProjectDto> projects, int totalCount)> GetAllProjectsForAdminAsync(ProjectRequestParametersForAdmin p, bool trackChanges);
        Task<IEnumerable<ProjectDto>> GetUsersProjectsAsync(string accountId, ProjectRequestParameters p, bool trackChanges);
        Task<Project?> GetProjectByIdAsync(Guid projectId, bool trackChanges);
        Task<Project?> GetProjectByIdForRestoreAsync(Guid projectId, bool trackChanges);
        void CreateProject(Project project);
        void UpdateProject(Project project);
    }
}
