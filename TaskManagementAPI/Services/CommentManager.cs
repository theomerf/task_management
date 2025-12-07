using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class CommentManager : ICommentService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;

        public CommentManager(IRepositoryManager manager, IMapper mapper, UserManager<Account> userManager)
        {
            _manager = manager;
            _mapper = mapper;
            _userManager = userManager;
        }

        private async Task<long> CommentAuthorizationCheck(string accountId, Guid projectId, string errorMessage, bool isAdmin, bool isCreate)
        {
            var (isAuthorized, id) = isCreate
                ? await _manager.Authorization.CanCreateCommentsAsync(accountId, projectId, isAdmin)
                : await _manager.Authorization.CanAccessCommentsAsync(accountId, projectId, isAdmin);

            if (!isAuthorized)
                throw new AccessViolationException(errorMessage);

            return id!.Value;
        }

        private bool CommentManageAuthorizationCheck(string accountId, CommentDtoForManage commentDto, string errorMessage, bool isAdmin)
        {
            if (isAdmin)
                return true;

            var isAuthorized = _manager.Authorization.CanManageCommentsAsync(accountId, commentDto);

            if (!isAuthorized)
                throw new AccessViolationException(errorMessage);

            return true;
        }

        // Comment
        public async Task<IEnumerable<CommentDto>> GetTaskCommentsAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev yorumlarına erişim yetkiniz yok.", isAdmin, false);
            var comments = await _manager.Comment.GetTaskCommentsAsync(id, taskId, false);
            var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);

            return commentsDto;
        }

        private async Task<Comment> GetCommentByIdForServiceAsync(long projectId, Guid taskId, Guid commentId, bool trackChanges)
        {
            var comment = await _manager.Comment.GetCommentByIdAsync(projectId, taskId, commentId, trackChanges);

            if (comment == null)
                throw new CommentNotFoundException(commentId);

            return comment;
        }

        public async Task<CommentDto> GetCommentByIdAsync(Guid projectId, Guid taskId, Guid commentId, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev yorumlarına erişim yetkiniz yok.", isAdmin, false);
            var comment = await GetCommentByIdForServiceAsync(id, taskId, commentId, false);
            var commentDto = _mapper.Map<CommentDto>(comment);

            return commentDto;
        }

        private async Task<CommentDtoForManage> GetCommentForManageAsync(Guid projectId, Guid taskId, Guid commentId)
        {
            var comment = await _manager.Comment.GetCommentByIdForManageAsync(projectId, taskId, commentId, true);

            if (comment == null)
                throw new CommentNotFoundException(commentId);

            return comment;
        }

        public async System.Threading.Tasks.Task CreateCommentAsync(Guid projectId, Guid taskId, CommentDtoForCreation commentDto, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev yorumu oluşturma yetkiniz yok.", isAdmin, true);
            var taskSequence = await _manager.Task.GetTaskIdAsync(id, taskId);

            var comment = _mapper.Map<Comment>(commentDto);
            comment.TaskId = taskSequence;
            comment.AuthorId = accountId;

            if (commentDto.ParentCommentId != null)
            {
                var parentCommentId = await _manager.Comment.GetCommentIdAsync(id, taskSequence, commentDto.ParentCommentId.Value);

                if (parentCommentId != null)
                    throw new CommentNotFoundException(commentDto.ParentCommentId.Value);

                comment.ParentCommentId = parentCommentId;
            }
            _manager.Comment.CreateComment(comment);

            var account = await _userManager.FindByIdAsync(accountId);

            var activiyLog = new ActivityLog
            {
                PerformedById = accountId,
                RelatedTaskId = taskSequence,
                RelatedProjectId = id,
                Type = ActivityType.CommentAdded,
                Description = $"{account!.Email} tarafından görev yorumu oluşturuldu.",
                CreatedAt = DateTime.UtcNow
            };
            _manager.ActivityLog.CreateActivityLog(activiyLog);

            await _manager.SaveAsync();
        }
        public async System.Threading.Tasks.Task UpdateCommentAsync(Guid projectId, Guid taskId, CommentDtoForUpdate commentDto, string accountId, bool isAdmin)
        {
            var comment = await GetCommentForManageAsync(projectId, taskId, commentDto.Id);
            CommentManageAuthorizationCheck(accountId, comment, "Bu görev yorumunu düzenleme yetkiniz yok.", isAdmin);

            _mapper.Map(commentDto, comment.Comment);

            var account = await _userManager.FindByIdAsync(accountId);

            var activiyLog = new ActivityLog
            {
                PerformedById = accountId,
                RelatedTaskId = comment.TaskId,
                RelatedProjectId = comment.ProjectSequence,
                Type = ActivityType.CommentUpdated,
                Description = $"{account!.Email} tarafından görev yorumu düzenlendi.",
                CreatedAt = DateTime.UtcNow
            };
            _manager.ActivityLog.CreateActivityLog(activiyLog);

            await _manager.SaveAsync();
        }
        public async System.Threading.Tasks.Task DeleteCommentAsync(Guid projectId, Guid taskId, Guid commentId, string accountId, bool isAdmin)
        {
            var comment = await GetCommentForManageAsync(projectId, taskId, commentId);
            CommentManageAuthorizationCheck(accountId, comment, "Bu görev yorumunu silme yetkiniz yok.", isAdmin);

            comment.Comment.DeletedAt = DateTime.UtcNow;

            var account = await _userManager.FindByIdAsync(accountId);
            var activiyLog = new ActivityLog
            {
                PerformedById = accountId,
                RelatedTaskId = comment.TaskId,
                RelatedProjectId = comment.ProjectSequence,
                Type = ActivityType.CommentDeleted,
                Description = $"{account!.Email} tarafından görev yorumu silindi.",
                CreatedAt = DateTime.UtcNow
            };
            _manager.ActivityLog.CreateActivityLog(activiyLog);

            await _manager.SaveAsync();
        }

        // Attachment
        public async Task<IEnumerable<AttachmentDto>> GetCommentAttachmentsAsync(Guid projectId, Guid taskId, Guid commentId, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev yorum eklerine erişim yetkiniz yok.", isAdmin, false);
            var attachments = await _manager.Comment.GetCommentAttachmentsAsync(id, taskId, commentId, false);

            return attachments;
        }

        private async Task<Attachment> GetCommentAttachmentByIdForServiceAsync(long projectId, Guid taskId, Guid attachmentId, bool trackChanges)
        {
            var attachment = await _manager.Comment.GetCommentAttachmentByIdAsync(projectId, taskId, attachmentId, false);
            if (attachment == null)
                throw new AttachmentNotFoundException(attachmentId);

            return attachment;
        }

        public async Task<AttachmentDetailsDto> GetCommentAttachmentByIdAsync(Guid projectId, Guid taskId, Guid commentId, Guid attachmentId, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev yorum eklerine erişim yetkiniz yok.", isAdmin, false);
            var attachment = await GetCommentAttachmentByIdForServiceAsync(id, taskId, attachmentId, false);
            var attachmentDto = _mapper.Map<AttachmentDetailsDto>(attachment);

            return attachmentDto;
        }
        public async System.Threading.Tasks.Task CreateCommentAttachmentAsync(Guid projectId, Guid taskId, Guid commentId, AttachmentDtoForCreation attachmentDto, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev yorumu eki oluşturma yetkiniz yok.", isAdmin, true);
            var taskSequence  = await _manager.Task.GetTaskIdAsync(id, taskId);
            var commentSequence = await _manager.Comment.GetCommentIdAsync(id, taskSequence, commentId);

            if (commentSequence == null)
                throw new CommentNotFoundException(commentId);

            var attachment = _mapper.Map<Attachment>(attachmentDto);
            attachment.TaskId = taskSequence;
            attachment.CommentId = commentSequence;
            _manager.Comment.CreateCommentAttachment(attachment);

            await _manager.SaveAsync(); 
        }
        public async System.Threading.Tasks.Task UpdateCommentAttachmentAsync(Guid projectId, Guid taskId, Guid commentId, AttachmentDtoForUpdate attachmentDto, string accountId, bool isAdmin)
        {
            var comment = await GetCommentForManageAsync(projectId, taskId, commentId);
            CommentManageAuthorizationCheck(accountId, comment, "Bu görev yorumu ekini düzenleme yetkiniz yok.", isAdmin);

            var attachment = await GetCommentAttachmentByIdForServiceAsync(comment.ProjectSequence, taskId, attachmentDto.Id, true);
            _mapper.Map(attachmentDto, attachment);

            await _manager.SaveAsync();
        }
        public async System.Threading.Tasks.Task DeleteCommentAttachmentAsync(Guid projectId, Guid taskId, Guid commentId, Guid attachmentId, string accountId, bool isAdmin)
        {
            var comment = await GetCommentForManageAsync(projectId, taskId, commentId);
            CommentManageAuthorizationCheck(accountId, comment, "Bu görev yorumu ekini silme yetkiniz yok.", isAdmin);

            var attachment = await GetCommentAttachmentByIdForServiceAsync(comment.ProjectSequence, taskId, attachmentId, true);
            attachment.DeletedAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        // Mention
        public async Task<IEnumerable<MentionDto>> GetCommentMentionsAsync(Guid projectId, Guid taskId, Guid commentId, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev yorum bahsetmelerine erişim yetkiniz yok.", isAdmin, false);
            var mentions = await _manager.Comment.GetCommentMentionsAsync(id, taskId, commentId, false);
            var mentionsDto = _mapper.Map<IEnumerable<MentionDto>>(mentions);

            return mentionsDto;
        }

        private async Task<Mention> GetMentionByIdForServiceAsync(long projectId, Guid taskId, Guid commentId, Guid mentionId, bool trackChanges)
        {
            var mention = await _manager.Comment.GetMentionByIdAsync(projectId, taskId, commentId, mentionId, trackChanges);

            if (mention == null)
                throw new MentionNotFoundException(mentionId);

            return mention;
        }
        public async Task<MentionDto> GetMentionByIdAsync(Guid projectId, Guid taskId, Guid commentId, Guid mentionId, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev yorum bahsetmelerine erişim yetkiniz yok.", isAdmin, false);
            var mention = await GetMentionByIdForServiceAsync(id, taskId, commentId, mentionId, false);
            var mentionDto = _mapper.Map<MentionDto>(mention);

            return mentionDto;
        }
        public async System.Threading.Tasks.Task CreateMentionAsync(Guid projectId, Guid taskId, Guid commentId, MentionDtoForCreation mentionDto, string accountId, bool isAdmin)
        {
            var id = await CommentAuthorizationCheck(accountId, projectId, "Bu projede görev bahsetmesi oluşturma yetkiniz yok.", isAdmin, true);
            var taskSequence = await _manager.Task.GetTaskIdAsync(id, taskId);
            var commentSequence = await _manager.Comment.GetCommentIdAsync(id, taskSequence, commentId);

            if (commentSequence == null)
                throw new CommentNotFoundException(commentId);

            var notification = new Notification
            {
                InitiatorId = accountId,
                RecipientId = mentionDto.MentionedUserId,
                RelatedTaskId = taskSequence,
                RelatedProjectId = id,
                Type = NotificationType.MentionAdded,
                Message = "Bir görev yorumunda sizden bahsedildi.",
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };
            _manager.Notification.CreateNotification(notification);

            var mention = _mapper.Map<Mention>(mentionDto);
            mention.CommentId = commentSequence.Value;
            _manager.Comment.CreateMention(mention);

            await _manager.SaveAsync();
        }
        public async System.Threading.Tasks.Task MarkMentionAsReadAsync(Guid projectId, Guid taskId, Guid commentId, Guid mentionId, string accountId)
        {
            var mention = await _manager.Authorization.CanMarkMentionAsReadAsync(accountId, projectId, taskId, commentId, mentionId);
            mention.ReadAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }
        public async System.Threading.Tasks.Task DeleteMentionAsync(Guid projectId, Guid taskId, Guid commentId, Guid mentionId, string accountId, bool isAdmin)
        {
            var comment = await GetCommentForManageAsync(projectId, taskId, commentId);
            CommentManageAuthorizationCheck(accountId, comment, "Bu görev yorumu bahsetmesini silme yetkiniz yok.", isAdmin);

            var mention = await GetMentionByIdForServiceAsync(comment.ProjectSequence, taskId, commentId, mentionId, false);
            _manager.Comment.DeleteMention(mention);

            await _manager.SaveAsync();
        }
    }
}
