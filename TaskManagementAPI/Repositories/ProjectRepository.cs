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

        public async Task<(IEnumerable<ProjectDto> projects, int totalCount)> GetAllProjectsForAdminAsync(ProjectRequestParametersForAdmin p, bool trackChanges)
        {
            var projectsQuery = FindAll(trackChanges)
                .FilterBy(p.Name, p => p.Name, FilterOperator.Contains)
                .FilterBy(p.Status, p => p.Status, FilterOperator.Equal)
                .FilterBy(p.Visibility, p => p.Visibility, FilterOperator.Equal);

            if (p.IsDeleted == true)
            {
                projectsQuery = projectsQuery
                    .IgnoreQueryFilters();
            }

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
                    CreatedByEmail = project.CreatedBy!.Email!,
                    MemberCount = project.Members.Count
                })
                .OrderByDescending(p => p.Id)
                .ToPaginate(p.PageNumber, p.PageSize)
                .ToListAsync();

            var totalCount = await projectsQuery.CountAsync();

            return (projects, totalCount);
        }

        public async Task<IEnumerable<ProjectDto>> GetUsersProjectsAsync(string accountId, ProjectRequestParameters p, bool trackChanges)
        {
            var projects = await FindByCondition(p => p.CreatedById == accountId || p.Members.Any(m => m.AccountId == accountId && m.LeftAt == null), trackChanges)
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
                    MemberCount = project.Members.Count
                })
                .OrderByDescending(p => p.Id)
                .ToListAsync();

            return projects;
        }

        public async Task<Project?> GetProjectByIdAsync(Guid projectId, bool trackChanges)
        {
            var project = await FindByCondition(p => p.Id == projectId, trackChanges)
                .Include(p => p.Tasks)
                .Include(p => p.Labels)
                .Include(p => p.Settings)
                .FirstOrDefaultAsync();

            return project;
        }

        public async Task<Project?> GetProjectByIdForRestoreAsync(Guid projectId, bool trackChanges)
        {
            var project = await FindByCondition(p => p.Id == projectId, trackChanges)
                .IgnoreQueryFilters()
                .Include(p => p.Tasks)
                .Include(p => p.Labels)
                .Include(p => p.Settings)
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
    }
}
