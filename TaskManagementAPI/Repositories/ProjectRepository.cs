using Entities.Dtos;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Extensions;

namespace Repositories
{
    public class ProjectRepository : RepositoryBase<Project>, IProjectRepository
    {
        public ProjectRepository(RepositoryContext context) : base(context)
        {
        }

        // Project

        public async Task<(IEnumerable<ProjectDto> projects, int totalCount)> GetAllProjectsForAdminAsync(ProjectRequestParametersForAdmin p, bool trackChanges)
        {
            var projectsQuery = FindAll(trackChanges)
                .Include(p => p.CreatedBy)
                .Include(p => p.Members)
                .AsSplitQuery()
                .FilterBy(p.Name, p => p.Name, FilterOperator.Contains)
                .FilterBy(p.Status, p => p.Status, FilterOperator.Equal)
                .FilterBy(p.Visibility, p => p.Visibility, FilterOperator.Equal);

            if (p.IsDeleted == true)
            {
                projectsQuery = projectsQuery
                    .IgnoreQueryFilters();
            }

            var totalCount = await projectsQuery.CountAsync();

            var projects = await projectsQuery
                .Select(project => new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Icon = project.Icon!,
                    Color = project.Color!,
                    Status = project.Status,
                    TaskCount = project.TaskCount,
                    CompletedTaskCount = project.CompletedTaskCount,
                    CreatedAt = project.CreatedAt,
                    CreatedByEmail = project.CreatedBy!.Email!,
                    MemberCount = project.Members.Count
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToPaginate(p.PageNumber, p.PageSize)
                .ToListAsync();

            return (projects, totalCount);
        }

        public async Task<IEnumerable<ProjectDto>> GetUsersProjectsAsync(string accountId, ProjectRequestParameters p, bool trackChanges)
        {
            var projects = await FindByCondition(p => p.CreatedById == accountId || p.Members.Any(m => m.AccountId == accountId && m.LeftAt == null), trackChanges)
                .Include(p => p.CreatedBy)
                .Include(p => p.Members)
                .AsSplitQuery()
                .FilterBy(p.Name, p => p.Name, FilterOperator.Contains)
                .FilterBy(p.Status, p => p.Status, FilterOperator.Equal)
                .FilterBy(p.Visibility, p => p.Visibility, FilterOperator.Equal)
                .FilterByRole(accountId, p.MemberRole)
                .Select(project => new ProjectDto
                {
                    Id = project.Id,
                    Name = project.Name,
                    Icon = project.Icon!,
                    Color = project.Color!,
                    Status = project.Status,
                    TaskCount = project.TaskCount,
                    CompletedTaskCount = project.CompletedTaskCount,
                    CreatedByEmail = project.CreatedBy!.Email!,
                    CreatedAt = project.CreatedAt,
                    MemberCount = project.Members.Count
                })
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return projects;
        }

        public async Task<Project?> GetProjectByIdAsync(Guid projectId, bool forDelete, bool trackChanges)
        {
            var projectQuery = FindByCondition(p => p.Id == projectId, trackChanges)
                .Include(p => p.Members);

            if (forDelete)
            {
                var projectForDelete = await projectQuery
                                .Include(p => p.Tasks)
                                    .ThenInclude(t => t.Comments)
                                .Include(p => p.Tasks)
                                    .ThenInclude(t => t.Attachments)
                                .Include(p => p.Tasks)
                                    .ThenInclude(t => t.TimeLogs)
                                .AsSplitQuery()
                                .FirstOrDefaultAsync();

                return projectForDelete;
            }

            var project = await projectQuery
                .FirstOrDefaultAsync();

            return project;
        }

        public async Task<Project?> GetProjectByIdForRestoreAsync(Guid projectId, bool trackChanges)
        {
            var project = await FindByCondition(p => p.Id == projectId, trackChanges)
                .IgnoreQueryFilters()
                .Include(p => p.Tasks)
                    .ThenInclude(t => t.Comments)
                .Include(p => p.Tasks)
                     .ThenInclude(t => t.Attachments)
                .Include(p => p.Tasks)
                     .ThenInclude(t => t.TimeLogs)
                .AsSplitQuery()
                .FirstOrDefaultAsync();

            return project;
        }

        public void CreateProject(Project project)
        {
            Create(project);
        }

        public void UpdateProject(Project project)
        {
            Update(project);
        }

        // Settings 

        public async Task<ProjectSetting?> GetProjectSettingsAsync(long projectId, bool trackChanges)
        {
            var settingsQuery = _context.ProjectSettings
                .Where(s => s.ProjectId == projectId);

            return await (trackChanges ? settingsQuery : settingsQuery.AsNoTracking())
                .FirstOrDefaultAsync();
        }

        public void UpdateProjectSettings(ProjectSetting settings)
        {
            _context.ProjectSettings.Update(settings);
        }

        // Label

        public async Task<IEnumerable<Label>> GetProjectLabelsAsync(long projectId, bool trackChanges)
        {
            var labelsQuery = _context.Labels
                .Where(l => l.ProjectId == projectId)
                .OrderBy(l => l.LabelSequence);

            return await (trackChanges ? labelsQuery : labelsQuery.AsNoTracking())
                .ToListAsync();
        }

        public async Task<Label?> GetLabelByIdAsync(long projectId, Guid labelId, bool trackChanges)
        {
            var labelQuery = _context.Labels
                .Where(l => l.ProjectId == projectId && l.Id == labelId);

            return await (trackChanges ? labelQuery : labelQuery.AsNoTracking())
                .FirstOrDefaultAsync();
        }

        public void CreateLabel(Label label)
        {
            _context.Labels.Add(label);
        }

        public void UpdateLabel(Label label)
        {
            _context.Labels.Update(label);
        }

        public void DeleteLabel(Label label)
        {
            _context.Labels.Remove(label);
        }

        // Member

        public async Task<IEnumerable<ProjectMember>> GetProjectMembersAsync(long projectId, bool trackChanges)
        {
            var membersQuery = _context.ProjectMembers
                .Include(m => m.Account)
                .Where(m => m.ProjectId == projectId)
                .OrderBy(m => m.Role == ProjectMemberRole.Owner ? 0 :
                              m.Role == ProjectMemberRole.Manager ? 1 : 2)
                .ThenBy(m => m.JoinedAt);

            return await (trackChanges ? membersQuery : membersQuery.AsNoTracking())
                .ToListAsync();
        }

        public async Task<ProjectMember?> GetProjectMemberByIdAsync(long projectId, Guid memberId, bool trackChanges)
        {
            var memberQuery = _context.ProjectMembers
                .Include(m => m.Account)
                .Where(m => m.ProjectId == projectId && m.Id == memberId);

            return await (trackChanges ? memberQuery : memberQuery.AsNoTracking())
                .FirstOrDefaultAsync();
        }

        public async Task<ProjectMember?> GetMemberByAccountIdAsync(long projectId, string accountId)
        {
            var member = await _context.ProjectMembers
                .Where(m => m.ProjectId == projectId && m.AccountId == accountId)
                .FirstOrDefaultAsync();

            return member;
        }

        public void CreateProjectMember(ProjectMember member)
        {
            _context.ProjectMembers.Add(member);
        }

        public void UpdateProjectMember(ProjectMember member)
        {
            _context.ProjectMembers.Update(member);
        }
    }
}
