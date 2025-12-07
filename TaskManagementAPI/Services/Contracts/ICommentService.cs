using Entities.Dtos;

namespace Services.Contracts
{
    public interface ICommentService
    {
        // Comment
        Task<IEnumerable<CommentDto>> GetTaskCommentsAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin);
        Task<CommentDto> GetCommentByIdAsync(Guid projectId, Guid taskId, Guid commentId, string accountId, bool isAdmin);
        Task CreateCommentAsync(Guid projectId, Guid taskId, CommentDtoForCreation commentDto, string accountId, bool isAdmin);
        Task UpdateCommentAsync(Guid projectId, Guid taskId, CommentDtoForUpdate commentDto, string accountId, bool isAdmin);
        Task DeleteCommentAsync(Guid projectId, Guid taskId, Guid commentId, string accountId, bool isAdmin);

        // Attachment
        Task<IEnumerable<AttachmentDto>> GetCommentAttachmentsAsync(Guid projectId, Guid taskId, Guid commentId, string accountId, bool isAdmin);
        Task<AttachmentDetailsDto> GetCommentAttachmentByIdAsync(Guid projectId, Guid taskId, Guid commentId, Guid attachmentId, string accountId, bool isAdmin);
        Task CreateCommentAttachmentAsync(Guid projectId, Guid taskId, Guid commentId, AttachmentDtoForCreation attachmentDto, string accountId, bool isAdmin);
        Task UpdateCommentAttachmentAsync(Guid projectId, Guid taskId, Guid commentId, AttachmentDtoForUpdate attachmentDto, string accountId, bool isAdmin);
        Task DeleteCommentAttachmentAsync(Guid projectId, Guid taskId, Guid commentId, Guid attachmentId, string accountId, bool isAdmin);

        // Mention
        Task<IEnumerable<MentionDto>> GetCommentMentionsAsync(Guid projectId, Guid taskId, Guid commentId, string accountId, bool isAdmin);
        Task<MentionDto> GetMentionByIdAsync(Guid projectId, Guid taskId, Guid commentId, Guid mentionId, string accountId, bool isAdmin);
        Task CreateMentionAsync(Guid projectId, Guid taskId, Guid commentId, MentionDtoForCreation mentionDto, string accountId, bool isAdmin);
        Task MarkMentionAsReadAsync(Guid projectId, Guid taskId, Guid commentId, Guid mentionId, string accountId);
        Task DeleteMentionAsync(Guid projectId, Guid taskId, Guid commentId, Guid mentionId, string accountId, bool isAdmin);
    }
}
