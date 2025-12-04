using Entities.Dtos;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Security.Claims;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IServiceManager _manager;

        public ProjectController(IServiceManager manager)
        {
            _manager = manager;
        }

        private bool IsAdmin => User.IsInRole("Admin");
        private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Project

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProjects([FromQuery] ProjectRequestParametersForAdmin p)
        {
            var projects = await _manager.ProjectService.GetAllProjectsForAdminAsync(p, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(projects.metaData));

            return Ok(projects.pagedProjects);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetProjectById([FromRoute] Guid id)
        {
            var project = await _manager.ProjectService.GetProjectByIdAsync(id, UserId!, IsAdmin);

            return Ok(project);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUsersProject([FromQuery] ProjectRequestParameters p)
        {
            var projects = await _manager.ProjectService.GetUsersProjectsAsync(UserId!, p);

            return Ok(projects);
        }

        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDtoForCreation projectDto)
        {
            var canCreate = User.IsInRole("Admin") || User.IsInRole("Manager");

            await _manager.ProjectService.CreateProjectAsync(projectDto, UserId!, canCreate);

            return StatusCode(201);
        }

        [HttpPut("update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectDtoForUpdate projectDto)
        {
            await _manager.ProjectService.UpdateProjectAsync(projectDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
        {
            await _manager.ProjectService.DeleteProjectAsync(id, UserId!, IsAdmin);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("restore/{id:guid}")]
        public async Task<IActionResult> RestoreProject([FromRoute] Guid id)
        {
            await _manager.ProjectService.RestoreProjectAsync(id);

            return Ok();
        }

        // Settings

        [HttpGet("{projectId:guid}/settings")]
        public async Task<IActionResult> GetProjectSettings([FromRoute] Guid projectId)
        {
            var settings = await _manager.ProjectService.GetProjectSettingsAsync(projectId, UserId!, IsAdmin);

            return Ok(settings);
        }

        [HttpPut("{projectId:guid}/settings/update")]
        public async Task<IActionResult> UpdateProjectSettings([FromRoute] Guid projectId, [FromBody] ProjectSettingDtoForUpdate settingsDto)
        {
            await _manager.ProjectService.UpdateProjectSettingsAsync(projectId, settingsDto, UserId!, IsAdmin);

            return Ok();
        }

        // Label

        [HttpGet("{projectId:guid}/labels")]
        public async Task<IActionResult> GetProjectLabels([FromRoute] Guid projectId)
        {
            var labels = await _manager.ProjectService.GetProjectLabelsAsync(projectId, UserId!, IsAdmin);

            return Ok(labels);
        }

        [HttpGet("{projectId:guid}/labels/{id:guid}")]
        public async Task<IActionResult> GetLabelById([FromRoute] Guid projectId, [FromRoute] Guid id)
        {
            var label = await _manager.ProjectService.GetLabelByIdAsync(projectId, id, UserId!, IsAdmin);

            return Ok(label);
        }

        [HttpPost("{projectId:guid}/labels/create")]
        public async Task<IActionResult> CreateLabelAsync([FromRoute] Guid projectId, [FromBody] LabelDtoForCreation labelDto)
        {
            await _manager.ProjectService.CreateLabelAsync(projectId, labelDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPut("{projectId:guid}/labels/update")]
        public async Task<IActionResult> UpdateLabelAsync([FromRoute] Guid projectId, [FromBody] LabelDtoForUpdate labelDto)
        {
            await _manager.ProjectService.UpdateLabelAsync(projectId, labelDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("{projectId:guid}/labels/delete/{id:guid}")]
        public async Task<IActionResult> DeleteLabelAsync([FromRoute] Guid projectId, [FromRoute] Guid id)
        {
            await _manager.ProjectService.DeleteLabelAsync(projectId, id, UserId!, IsAdmin);

            return NoContent();
        }

        // Member

        [HttpGet("{projectId:guid}/members")]
        public async Task<IActionResult> GetProjectMembers([FromRoute] Guid projectId)
        {
            var members = await _manager.ProjectService.GetProjectMembersAsync(projectId, UserId!, IsAdmin);

            return Ok(members);
        }

        [HttpGet("{projectId:guid}/members/{id:guid}")]
        public async Task<IActionResult> GetProjectMemberById([FromRoute] Guid projectId, [FromRoute] Guid id)
        {
            var member = await _manager.ProjectService.GetProjectMemberByIdAsync(projectId, id, UserId!, IsAdmin);

            return Ok(member);
        }

        [HttpPost("{projectId:guid}/members/add")]
        public async Task<IActionResult> AddMemberAsync([FromRoute] Guid projectId, [FromBody] ProjectMemberDtoForCreation memberDto)
        {
            await _manager.ProjectService.AddMemberAsync(projectId, memberDto, UserId!, IsAdmin);

            return StatusCode(201);
        }

        [HttpPut("{projectId:guid}/members/update")]
        public async Task<IActionResult> UpdateMemberAsync([FromRoute] Guid projectId, [FromBody] ProjectMemberDtoForUpdate memberDto)
        {
            await _manager.ProjectService.UpdateMemberAsync(projectId, memberDto, UserId!, IsAdmin);

            return Ok();
        }

        [HttpDelete("{projectId:guid}/members/remove/{id:guid}")]
        public async Task<IActionResult> RemoveMemberAsync([FromRoute] Guid projectId, [FromRoute] Guid id)
        {
            await _manager.ProjectService.RemoveMemberAsync(projectId, id, UserId!, IsAdmin);

            return NoContent();
        }
    }
}
