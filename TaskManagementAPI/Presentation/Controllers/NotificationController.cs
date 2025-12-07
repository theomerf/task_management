using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public NotificationController(IServiceManager manager)
        {
            _manager = manager;
        }
        
        private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet("me")]
        public async Task<IActionResult> GetUserNotifications()
        {
            var notifications = await _manager.NotificationService.GetUserNotificationsAsync(UserId!);

            return Ok(notifications);
        }

        [HttpPatch("{notificationId:guid}/mark-as-read")]
        public async Task<IActionResult> MarkNotificationAsRead([FromRoute] Guid notificationId)
        {
            await _manager.NotificationService.MarkNotificationAsReadAsync(UserId!, notificationId);


            return NoContent();
        }

        [HttpPatch("{notificationId:guid}/archive")]
        public async Task<IActionResult> ArchiveNotification([FromRoute] Guid notificationId)
        {
            await _manager.NotificationService.ArchiveNotificationAsync(UserId!, notificationId);

            return NoContent();
        }
    }
}
