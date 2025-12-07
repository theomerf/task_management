using Entities.Dtos;
using Entities.Models;

namespace Repositories.Contracts
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        // Comment
        Task<IEnumerable<Comment>> GetTaskCommentsAsync(long projectId, Guid taskId, bool trackChanges);
        Task<Comment?> GetCommentByIdAsync(long projectId, Guid taskId, Guid commentId, bool trackChanges);
        Task<long?> GetCommentIdAsync(long projectId, long taskId, Guid commentId);
        Task<CommentDtoForManage?> GetCommentByIdForManageAsync(Guid projectId, Guid taskId, Guid commentId, bool trackChanges);
        void CreateComment(Comment comment);
        void UpdateComment(Comment comment);

        // Attachment
        Task<IEnumerable<AttachmentDto>> GetCommentAttachmentsAsync(long projectId, Guid taskId, Guid commentId, bool trackChanges);
        Task<Attachment?> GetCommentAttachmentByIdAsync(long projectId, Guid taskId, Guid attachmentId, bool trackChanges);
        void CreateCommentAttachment(Attachment attachment);
        void UpdateCommentAttachment(Attachment attachment);

        // Mention
        Task<IEnumerable<Mention>> GetCommentMentionsAsync(long projectId, Guid taskId, Guid commentId, bool trackChanges);
        Task<Mention?> GetMentionByIdAsync(long projectId, Guid taskId, Guid commentId, Guid mentionId, bool trackChanges);
        void CreateMention(Mention mention);
        void UpdateMention(Mention mention);
        void DeleteMention(Mention mention);
    }
}
