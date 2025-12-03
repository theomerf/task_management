using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/project/{projectId:guid}/tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public TaskController(IServiceManager manager)
        {
            _manager = manager;
        }

        private bool IsAdmin => User.IsInRole("Admin");
        private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Task

        [HttpGet]
        public async Task<IActionResult> GetProjectTasks([FromRoute] Guid projectId, [FromQuery] TaskRequestParameters p)
        {
            var tasks = await _manager.TaskService.GetProjectTasksAsync(projectId, p, UserId!, IsAdmin);

            return Ok(tasks);
        }

        [HttpGet("{taskId:guid}")]
        public async Task<IActionResult> GetTaskById([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var task = await _manager.TaskService.GetTaskByIdAsync(projectId, taskId, UserId!, IsAdmin);

            return Ok(task);
        }
    }
}
