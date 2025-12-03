using Entities.Dtos;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface IProjectService
    {
        // Project
        Task<(PagedList<ProjectDto> pagedProjects, MetaData metaData)> GetAllProjectsForAdminAsync(ProjectRequestParametersForAdmin p, bool trackChanges);
        Task<IEnumerable<ProjectDto>> GetUsersProjectsAsync(string accountId, ProjectRequestParameters p);
        Task<ProjectDetailsDto> GetProjectByIdAsync(Guid projectId, string accountId, bool isAdmin);
        Task CreateProjectAsync(ProjectDtoForCreation projectDto, string accountId, bool canCreate);
        Task UpdateProjectAsync(ProjectDtoForUpdate projectDto, string accountId, bool isAdmin);
        Task DeleteProjectAsync(Guid projectId, string accountId, bool isAdmin);
        Task RestoreProjectAsync(Guid projectId);

        // Settings
        Task<ProjectSettingDto> GetProjectSettingsAsync(Guid projectId, string accountId, bool isAdmin);
        Task UpdateProjectSettingsAsync(Guid projectId, ProjectSettingDtoForUpdate settingsDto, string accountId, bool isAdmin);

        // Label
        Task<IEnumerable<LabelDto>> GetProjectLabelsAsync(Guid projectId, string accountId, bool isAdmin);
        Task<LabelDto> GetLabelByIdAsync(Guid projectId, Guid labelId, string accountId, bool isAdmin);
        Task CreateLabelAsync(Guid projectId, LabelDtoForCreation labelDto, string accountId, bool isAdmin);
        Task UpdateLabelAsync(Guid projectId, LabelDtoForUpdate labelDto, string accountId, bool isAdmin);
        Task DeleteLabelAsync(Guid projectId, Guid labelId, string accountId, bool isAdmin);

        // Member
        Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(Guid projectId, string accountId, bool isAdmin);
        Task<ProjectMemberDto> GetProjectMemberByIdAsync(Guid projectId, Guid memberId, string accountId, bool isAdmin);
        Task AddMemberAsync(Guid projectId, ProjectMemberDtoForCreation memberDto, string accountId, bool isAdmin);
        Task UpdateMemberAsync(Guid projectId, ProjectMemberDtoForUpdate memberDto, string accountId, bool isAdmin);
        Task RemoveMemberAsync(Guid projectId, Guid memberId, string accountId, bool isAdmin);
    }
}
