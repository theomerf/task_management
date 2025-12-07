using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/{projectId:guid}/[controller]")]
    [Authorize]
    public class ActivityLogController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public ActivityLogController(IServiceManager manager)
        {
            _manager = manager;
        }

        private bool IsAdmin => User.IsInRole("Admin");
        private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<IActionResult> GetProjectActivityLogs([FromRoute] Guid projectId, [FromQuery] ActivityLogRequestParameters p)
        {
            var activityLogs = await _manager.ActivityLogService.GetProjectActiviyLogsAsync(projectId, p, UserId!, IsAdmin);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(activityLogs.metaData));

            return Ok(activityLogs.pagedActivityLogs);
        }
    }
}
