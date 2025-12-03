using Entities.Dtos;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts
{
    public interface IProjectRepository : IRepositoryBase<Project>
    {
        // Project
        Task<(IEnumerable<ProjectDto> projects, int totalCount)> GetAllProjectsForAdminAsync(ProjectRequestParametersForAdmin p, bool trackChanges);
        Task<IEnumerable<ProjectDto>> GetUsersProjectsAsync(string accountId, ProjectRequestParameters p, bool trackChanges);
        Task<Project?> GetProjectByIdAsync(Guid projectId, bool trackChanges);
        Task<Project?> GetProjectByIdForRestoreAsync(Guid projectId, bool trackChanges);
        void CreateProject(Project project);
        void UpdateProject(Project project);

        // Settings
        Task<ProjectSetting?> GetProjectSettingsAsync(long projectId, bool trackChanges);
        void UpdateProjectSettings(ProjectSetting settings);

        // Label
        Task<IEnumerable<Label>> GetProjectLabelsAsync(long projectId, bool trackChanges);
        Task<Label?> GetLabelByIdAsync(long projectId, Guid labelId, bool trackChanges);
        void CreateLabel(Label label);
        void UpdateLabel(Label label);
        void DeleteLabel(Label label);

        // Member
        Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(long projectId, bool trackChanges);
        Task<ProjectMember?> GetProjectMemberByIdAsync(long projectId, Guid memberId, bool trackChanges);
        Task<ProjectMember?> GetMemberByAccountIdAsync(long projectId, string accountId);
        void CreateProjectMember(ProjectMember member);
        void UpdateProjectMember(ProjectMember member);
    }
}
