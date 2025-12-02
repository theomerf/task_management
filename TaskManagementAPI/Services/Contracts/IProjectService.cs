using Entities.Dtos;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IProjectService
    {
        Task<(PagedList<ProjectDto> pagedProjects, MetaData metaData)> GetAllProjectsForAdminAsync(ProjectRequestParametersForAdmin p, bool trackChanges);
        Task<IEnumerable<ProjectDto>> GetUsersProjectsAsync(string accountId, ProjectRequestParameters p);
        Task<ProjectDetailsDto> GetProjectByIdAsync(Guid projectId, string accountId, bool isAdmin);
        Task CreateProjectAsync(ProjectDtoForCreation projectDto, bool canCreate);
        Task UpdateProjectAsync(ProjectDtoForUpdate projectDto, string accountId, bool isAdmin);
        Task DeleteProjectAsync(Guid projectId, string accountId, bool isAdmin);
        Task RestoreProjectAsync(Guid projectId);
    }
}
