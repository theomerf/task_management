using Entities.Dtos;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/project/{projectId:guid}/task")]
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

        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTask([FromRoute] Guid projectId, [FromBody] Entities.Dtos.TaskDtoForCreation taskDto)
        {
            await _manager.TaskService.CreateTaskAsync(projectId, taskDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPut("update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTask([FromRoute] Guid projectId, [FromBody] TaskDtoForUpdate taskDto)
        {
            await _manager.TaskService.UpdateTaskAsync(projectId, taskDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpPatch("update-status")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTaskStatus([FromRoute] Guid projectId, [FromBody] TaskDtoForStatusUpdate taskDto)
        {
            await _manager.TaskService.UpdateTaskStatusAsync(projectId, taskDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpPatch("update-priority")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTaskPriority([FromRoute] Guid projectId, [FromBody] TaskDtoForPriorityUpdate taskDto)
        {
            await _manager.TaskService.UpdateTaskPriorityAsync(projectId, taskDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("delete/{taskId:guid}")] 
        public async Task<IActionResult> DeleteTask([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            await _manager.TaskService.DeleteTaskAsync(projectId, taskId, UserId!, IsAdmin);

            return NoContent();
        }

        // Attachment

        [HttpGet("{taskId:guid}/attachment")]
        public async Task<IActionResult> GetTaskAttachments([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var taskAttachments = await _manager.TaskService.GetTaskAttachmentsAsync(projectId, taskId, UserId!, IsAdmin);

            return Ok(taskAttachments);
        }

        [HttpGet("{taskId:guid}/attachment/{attachmentId:guid}")]
        public async Task<IActionResult> GetTaskAttachmentById([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid attachmentId)
        {
            var taskAttachment = await _manager.TaskService.GetTaskAttachmentByIdAsync(projectId, taskId, attachmentId, UserId!, IsAdmin);

            return Ok(taskAttachment);
        }

        [HttpPost("attachment/create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTaskAttachment([FromRoute] Guid projectId, [FromBody] AttachmentDtoForCreation attachmentDto)
        {
            await _manager.TaskService.CreateTaskAttachmentAsync(projectId, attachmentDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPut("attachment/update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTaskAttachment([FromRoute] Guid projectId, [FromBody] AttachmentDtoForUpdate attachmentDto)
        {
            await _manager.TaskService.UpdateTaskAttachmentAsync(projectId, attachmentDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("{taskId:guid}/attachment/delete/{attachmentId:guid}")]
        public async Task<IActionResult> DeleteTaskAttachment([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid attachmentId)
        {
            await _manager.TaskService.DeleteTaskAttachmentAsync(projectId, taskId, attachmentId, UserId!, IsAdmin);

            return NoContent();
        }

        // TimeLog

        [HttpGet("{taskId:guid}/timelog")]
        public async Task<IActionResult> GetTaskTimeLogs([FromRoute] Guid projectId, [FromRoute] Guid taskId)
        {
            var timeLogs = await _manager.TaskService.GetTaskTimeLogsAsync(projectId, taskId, UserId!, IsAdmin);

            return Ok(timeLogs);
        }

        [HttpGet("{taskId:guid}/timelog/{timeLogId:guid}")]
        public async Task<IActionResult> GetTimeLogById([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid timeLogId)
        {
            var timeLog = await _manager.TaskService.GetTimeLogByIdAsync(projectId, taskId, timeLogId, UserId!, IsAdmin);

            return Ok(timeLog);
        }

        [HttpPost("timelog/create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTimeLog([FromRoute] Guid projectId, [FromBody] TimeLogDtoForCreation timeLogDto)
        {
            await _manager.TaskService.CreateTimeLogAsync(projectId, timeLogDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPut("timelog/update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTimeLog([FromRoute] Guid projectId, [FromBody] TimeLogDtoForUpdate timeLogDto)
        {
            await _manager.TaskService.UpdateTimeLogAsync(projectId, timeLogDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("{taskId:guid}/timelog/delete/{timeLogId:guid}")]
        public async Task<IActionResult> DeleteTimeLog([FromRoute] Guid projectId, [FromRoute] Guid taskId, [FromRoute] Guid timeLogId)
        {
            await _manager.TaskService.DeleteTimeLogAsync(projectId, taskId, timeLogId, UserId!, IsAdmin);

            return NoContent();
        }

        // TimeLogCategory

        [HttpGet("timelog-category")]
        [ResponseCache(Duration = 300)]
        public async Task<IActionResult> GetProjectTimeLogCategories([FromRoute] Guid projectId)
        {
            var categories = await _manager.TaskService.GetProjectsTimeLogCategoriesAsync(projectId, UserId!, IsAdmin);

            return Ok(categories);
        }

        [HttpGet("timelog-category/{categoryId:guid}")]
        public async Task<IActionResult> GeTimeLogCategoryById([FromRoute] Guid projectId, [FromRoute] Guid categoryId)
        {
            var category = await _manager.TaskService.GetTimeLogCategoryByIdAsync(projectId, categoryId, UserId!, IsAdmin);

            return Ok(category);
        }

        [HttpPost("timelog-category/create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTimeLogCategory([FromRoute] Guid projectId, [FromBody] TimeLogCategoryDtoForCreation categoryDto)
        {
            await _manager.TaskService.CreateTimeLogCategoryAsync(projectId, categoryDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPut("timelog-category/update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateTimeLogCategory([FromRoute] Guid projectId, [FromBody] TimeLogCategoryDtoForUpdate categoryDto)
        {
            await _manager.TaskService.UpdateTimeLogCategoryAsync(projectId, categoryDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("timelog-category/delete/{categoryId:guid}")]
        public async Task<IActionResult> DeleteTimeLogCategory([FromRoute] Guid projectId, [FromRoute] Guid categoryId)
        {
            await _manager.TaskService.DeleteTimeLogCategoryAsync(projectId, categoryId, UserId!, IsAdmin);

            return NoContent();
        }
    }
}
