using Entities.Dtos;
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

        public async Task<(bool isAuthorized, long? id)> CanAccessCommentsAsync(string accountId, Guid projectId, bool isAdmin)
        {
            var project = await FindByCondition(p => p.Id == projectId, false)
                .Include(p => p.Members)
                .Select(g => new
                {
                    Id = g.ProjectSequence,
                    CreatedById = g.CreatedById,
                    Visibility = g.Visibility,
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

        public async Task<(bool isAuthorized, long? id)> CanCreateCommentsAsync(string accountId, Guid projectId, bool isAdmin)
        {
            var project = await FindByCondition(p => p.Id == projectId, false)
                .Include(p => p.Members)
                .Select(g => new
                {
                    Id = g.ProjectSequence,
                    CreatedById = g.CreatedById,
                    Visibility = g.Visibility,
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
                    return (false, null);
                case ProjectVisibility.Private:
                    return project.CreatedById == accountId ? (true, project.Id) : (false, null);
                case ProjectVisibility.Team:
                    return project.CreatedById == accountId || project.Members.Any(m => m.AccountId == accountId) ? (true, project.Id) : (false, null);
                default:
                    return (false, null);
            }
        }

        public bool CanManageCommentsAsync(string accountId, CommentDtoForManage comment)
        {      
            if (comment.ProjectStatus == ProjectStatus.Archived)
                throw new Exception("Proje arşivlenmiş durumdadır.");

            return comment.ProjectCreatedById == accountId || comment.AuthorId == accountId ? true : false;
        }

        public async Task<Mention> CanMarkMentionAsReadAsync(string accountId, Guid projectId, Guid taskId, Guid commentId, Guid mentionId)
        {
            var mention = await _context.Mentions
                .Where(m => m.Comment!.Task!.Project!.Id == projectId && m.Comment.Task.Id == taskId && m.Comment.Id == commentId && m.Id == mentionId)
                .FirstOrDefaultAsync();

            if (mention == null)
                throw new MentionNotFoundException(mentionId);

            if (mention.MentionedUserId == accountId && mention.ReadAt == null)
                return mention;

            throw new UnauthorizedAccessException("Bu mention'ı okuma yetkiniz yok.");
        }

        public async Task<(bool isAuthorized, long? id)> CanAccessActivityLogsAsync(string accountId, Guid projectId, bool isAdmin)
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
                    return project.CreatedById == accountId || project.Members.Any(m => m.AccountId == accountId && m.Role != ProjectMemberRole.Member) ? (true, project.Id) : (false, null);
                default:
                    return (false, null);
            }
        }
    }
}
