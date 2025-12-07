using Entities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/project/{projectId:guid}/task/{taskId:guid}/comment")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public CommentController(IServiceManager manager)
        {
            _manager = manager;
        }

        private bool IsAdmin => User.IsInRole("Admin");
        private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Comment

        [HttpGet]
        public async Task<IActionResult> GetTaskComments([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var comments = await _manager.CommentService.GetTaskCommentsAsync(projectId, taskId, UserId!, IsAdmin);

            return Ok(comments);
        }

        [HttpGet("{commentId:guid}")]
        public async Task<IActionResult> GetCommentById([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId)
        {
            var comment = await _manager.CommentService.GetCommentByIdAsync(projectId, taskId, commentId, UserId!, IsAdmin);

            return Ok(comment);
        }

        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateComment([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] CommentDtoForCreation commentDto)
        {
            await _manager.CommentService.CreateCommentAsync(projectId, taskId, commentDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPut("update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateComment([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromBody] CommentDtoForUpdate commentDto)
        {
            await _manager.CommentService.UpdateCommentAsync(projectId, taskId, commentDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("delete/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId)
        {
            await _manager.CommentService.DeleteCommentAsync(projectId, taskId, commentId, UserId!, IsAdmin);

            return NoContent();
        }

        // Attachment

        [HttpGet("{commentId:guid}/attachment")]
        public async Task<IActionResult> GetCommentAttachments([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId)
        {
            var attachments = await _manager.CommentService.GetCommentAttachmentsAsync(projectId, taskId, commentId, UserId!, IsAdmin);

            return Ok(attachments);
        }

        [HttpGet("{commentId:guid}/attachment/{attachmentId:guid}")]
        public async Task<IActionResult> GetCommentAttachmentById([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId, [FromRoute] Guid attachmentId)
        {
            var attachment = await _manager.CommentService.GetCommentAttachmentByIdAsync(projectId, taskId, commentId, attachmentId, UserId!, IsAdmin);

            return Ok(attachment);
        }

        [HttpPost("{commentId:guid}/attachment/create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCommentAttachment([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId, [FromBody] AttachmentDtoForCreation attachmentDto)
        {
            await _manager.CommentService.CreateCommentAttachmentAsync(projectId, taskId, commentId, attachmentDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPut("{commentId:guid}/attachment/update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateCommentAttachment([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId, [FromBody] AttachmentDtoForUpdate attachmentDto)
        {
            await _manager.CommentService.UpdateCommentAttachmentAsync(projectId, taskId, commentId, attachmentDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("{commentId:guid}/attachment/delete/{attachmentId:guid}")]
        public async Task<IActionResult> DeleteCommentAttachment([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId, [FromRoute] Guid attachmentId)
        {
            await _manager.CommentService.DeleteCommentAttachmentAsync(projectId, taskId, commentId, attachmentId, UserId!, IsAdmin);

            return NoContent();
        }


        // Mention

        [HttpGet("{commentId:guid}/mention")]
        public async Task<IActionResult> GetCommentMentions([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId)
        {
            var mentions = await _manager.CommentService.GetCommentMentionsAsync(projectId, taskId, commentId, UserId!, IsAdmin);

            return Ok(mentions);
        }

        [HttpGet("{commentId:guid}/mention/{mentionId:guid}")]
        public async Task<IActionResult> GetMentionById([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId, [FromRoute] Guid mentionId)
        {
            var mention = await _manager.CommentService.GetMentionByIdAsync(projectId, taskId, commentId, mentionId, UserId!, IsAdmin);

            return Ok(mention);
        }

        [HttpPost("{commentId:guid}/mention/create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateMention([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId, [FromBody] MentionDtoForCreation mentionDto)
        {
            await _manager.CommentService.CreateMentionAsync(projectId, taskId, commentId, mentionDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPatch("{commentId:guid}/mention/mark-as-read/{mentionId:guid}")]
        public async Task<IActionResult> MarkMentionAsRead([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId, [FromRoute] Guid mentionId)
        {
            await _manager.CommentService.MarkMentionAsReadAsync(projectId, taskId, commentId, mentionId, UserId!);

            return Ok();
        }

        [HttpDelete("{commentId:guid}/mention/delete/{mentionId:guid}")]
        public async Task<IActionResult> DeleteMention([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid commentId, [FromRoute] Guid mentionId)
        {
            await _manager.CommentService.DeleteMentionAsync(projectId, taskId, commentId, mentionId, UserId!, IsAdmin);

            return NoContent();
        }
    }
}
