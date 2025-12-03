using Entities.Exceptions;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class AuthorizationRepository : RepositoryBase<Project>, IAuthorizationRepository
    {
        public AuthorizationRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<bool> CanAccessProjectAsync(string accountId, Project project)
        {
            if (project.Status == ProjectStatus.Archived)
                throw new Exception("Proje arşivlenmiş durumdadır.");

            switch (project.Visibility)
            {
                case ProjectVisibility.Public:
                    return true;
                case ProjectVisibility.Private:
                    return project.CreatedById == accountId ? true : false;
                case ProjectVisibility.Team:
                    return project.CreatedById == accountId || project.Members.Any(m => m.AccountId == accountId);
                default:
                    return false;
            }
        }

        public async Task<bool> CanManageProjectAsync(string accountId, Project project)
        {
            if (project.Status == ProjectStatus.Archived)
                throw new Exception("Proje arşivlenmiş durumdadır.");

            return project.CreatedById == accountId || project.Members.Any(m => m.AccountId == accountId && m.Role != ProjectMemberRole.Member);
        }

        public async Task<bool> CanDeleteProjectAsync(string accountId, Project project)
        {
            if (project.Status == ProjectStatus.Archived)
                throw new Exception("Proje arşivlenmiş durumdadır.");

            return project.CreatedById == accountId || project.Members.Any(m => m.AccountId == accountId && m.Role == ProjectMemberRole.Owner);
        }

        public async Task<(bool isAuthorized, long? id)> CanAccessTasksAsync(string accountId, Guid projectId, bool isAdmin)
        {
            var project = await FindByCondition(p => p.Id == projectId, false)
                .Include(p => p.Members)
                .Select(g => new
                {
                    Id = g.ProjectSequence,
                    Visibility = g.Visibility,
                    CreatedById = g.CreatedById,
                    Members = g.Members,
                    Status = g.Status
                })
                .FirstOrDefaultAsync();

            if (project == null)
                throw new ProjectNotFoundException(projectId);

            if (isAdmin)
                return (true, project?.Id);

            if (project.Status == ProjectStatus.Archived)
                throw new Exception("Proje arşivlenmiş durumdadır.");

            switch (project.Visibility)
            {
                case ProjectVisibility.Public:
                    return (true, project.Id);
                case ProjectVisibility.Private:
                    return project.CreatedById == accountId ? (true, project.Id) : (false, null);
                case ProjectVisibility.Team:
                    return project.CreatedById == accountId || project.Members.Any(m => m.AccountId == accountId) ? (true, project.Id) : (false, null);
                default:
                    return (false, null);
            }
        }

        public async Task<(bool isAuthorized, long? id)> CanManageTasksAsync(string accountId, Guid projectId, bool isAdmin)
        {
            var project = await FindByCondition(p => p.Id == projectId, false)
                .Include(p => p.Members)
                .Select(g => new
                {
                    Id = g.ProjectSequence,
                    CreatedById = g.CreatedById,
                    Members = g.Members,
                    Status = g.Status
                })
                .FirstOrDefaultAsync();

            if (project == null)
                throw new ProjectNotFoundException(projectId);

            if (isAdmin)
                return (true, project?.Id);

            if (project.Status == ProjectStatus.Archived)
                throw new Exception("Proje arşivlenmiş durumdadır.");

            return project.CreatedById == accountId || project.Members.Any(m => m.AccountId == accountId && m.Role != ProjectMemberRole.Member) ? (true, project.Id) : (false, null);
        }
    }
}
