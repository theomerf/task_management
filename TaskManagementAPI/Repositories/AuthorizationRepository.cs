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
    }
}
