using Entities.Dtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(RepositoryContext context) : base(context)
        {
        }

        // Comment

        public async Task<IEnumerable<Comment>> GetTaskCommentsAsync(long projectId, Guid taskId, bool trackChanges)
        {
            var comments = await FindByCondition(c => c.Task!.ProjectId == projectId &&  c.Task.Id == taskId, trackChanges)
                .Include(c => c.Author)
                .ToListAsync();

            return comments;
        }

        public async Task<Comment?> GetCommentByIdAsync(long projectId, Guid taskId, Guid commentId, bool trackChanges)
        {
            var comment = await FindByCondition(c => c.Task!.ProjectId == projectId && c.Task.Id == taskId && c.Id == commentId, trackChanges)
                .Include(c => c.Author)
                .FirstOrDefaultAsync();

            return comment;
        }

        public async Task<long?> GetCommentIdAsync(long projectId, long taskId, Guid commentId)
        {
            var id = await FindByCondition(c => c.Task!.ProjectId == projectId && c.TaskId == taskId && c.Id == commentId, false)
                .Select(c => c.CommentSequence)
                .FirstOrDefaultAsync();
            
            return id == 0 ? null : id;
        }

        public async Task<CommentDtoForManage?> GetCommentByIdForManageAsync(Guid projectId, Guid taskId, Guid commentId, bool trackChanges)
        {
            var comment = await FindByCondition(c => c.Task!.Project!.Id == projectId && c.Task.Id == taskId && c.Id == commentId, trackChanges)
                .Select(c => new CommentDtoForManage
                {
                    Comment = c,
                    AuthorId = c.AuthorId!,
                    TaskId = c.TaskId,
                    ProjectSequence = c.Task!.Project!.ProjectSequence,
                    ProjectStatus = c.Task!.Project!.Status,
                    ProjectCreatedById = c.Task!.Project!.CreatedById!
                })
                .FirstOrDefaultAsync();

            return comment;
        }

        public void CreateComment(Comment comment)
        {
            Create(comment);
        }

        public void UpdateComment(Comment comment)
        {
            Update(comment);
        }

        // Attachment

        public async Task<IEnumerable<AttachmentDto>> GetCommentAttachmentsAsync(long projectId, Guid taskId, Guid commentId, bool trackChanges)
        {
            var attachmentsQuery = _context.Attachments
                .Where(a => a.Comment!.Task!.ProjectId == projectId && a.Comment.Task.Id == taskId && a.Comment.Id == commentId)
                .Select(a => new AttachmentDto
                {
                    Id = a.Id,
                    FileUrl = a.FileUrl,
                    ThumbnailUrl = a.ThumbnailUrl,
                    UploadedAt = a.UploadedAt,
                    UploadedByEmail = a.UploadedBy!.Email!
                });

            return await (trackChanges ? attachmentsQuery : attachmentsQuery.AsNoTracking())
                .ToListAsync();
        }

        public async Task<Attachment?> GetCommentAttachmentByIdAsync(long projectId, Guid taskId, Guid attachmentId, bool trackChanges)
        {
            var attachmentQuery = _context.Attachments
                .Include(a => a.UploadedBy)
                .Where(a => a.Comment!.Task!.ProjectId == projectId && a.Comment.Task.Id == taskId && a.Id == attachmentId);

            return await (trackChanges ? attachmentQuery : attachmentQuery.AsNoTracking())
                .FirstOrDefaultAsync();
        }

        public void CreateCommentAttachment(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
        }

        public void UpdateCommentAttachment(Attachment attachment)
        {
            _context.Attachments.Update(attachment);
        }

        // Mention

        public async Task<IEnumerable<Mention>> GetCommentMentionsAsync(long projectId, Guid taskId, Guid commentId, bool trackChanges)
        {
            var mentionsQuery = _context.Mentions
                .Include(m => m.MentionedUser)
                .Where(m => m.Comment!.Task!.ProjectId == projectId && m.Comment.Task.Id == taskId && m.Comment.Id == commentId);

            return await (trackChanges ? mentionsQuery : mentionsQuery.AsNoTracking())
                .ToListAsync();
        }

        public async Task<Mention?> GetMentionByIdAsync(long projectId, Guid taskId, Guid commentId, Guid mentionId, bool trackChanges)
        {
            var mentionQuery = _context.Mentions
                .Include(m => m.MentionedUser)
                .Where(m => m.Comment!.Task!.ProjectId == projectId && m.Comment.Task.Id == taskId && m.Comment.Id == commentId && m.Id == mentionId);

            return await (trackChanges ? mentionQuery : mentionQuery.AsNoTracking())
                .FirstOrDefaultAsync();
        }

        public void CreateMention(Mention mention)
        {
            _context.Mentions.Add(mention);
        }

        public void UpdateMention(Mention mention)
        {
            _context.Mentions.Update(mention);
        }

        public void DeleteMention(Mention mention)
        {
            _context.Mentions.Remove(mention);
        }
    }
}
