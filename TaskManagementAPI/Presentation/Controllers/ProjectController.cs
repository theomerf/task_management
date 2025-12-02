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

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllProjects([FromQuery] ProjectRequestParametersForAdmin p)
        {
            var projects = await _manager.ProjectService.GetAllProjectsForAdminAsync(p, false);
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(projects.metaData));

            return Ok(projects.pagedProjects);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOneProject([FromRoute] Guid id)
        {
            var isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var project = await _manager.ProjectService.GetProjectByIdAsync(id, userId!, isAdmin);

            return Ok(project);
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetUsersProject([FromQuery] ProjectRequestParameters p)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projects = await _manager.ProjectService.GetUsersProjectsAsync(userId!, p);

            return Ok(projects);
        }

        [HttpPost("create")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateProject([FromBody] ProjectDtoForCreation projectDto) 
        {
            var canCreate = User.IsInRole("Admin") || User.IsInRole("Manager");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            projectDto.CreatedById = userId;
            await _manager.ProjectService.CreateProjectAsync(projectDto, canCreate);

            return StatusCode(201);
        }

        [HttpPut("update")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> UpdateProject([FromBody] ProjectDtoForUpdate projectDto)
        {
            var isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            projectDto.CreatedById = userId;
            await _manager.ProjectService.UpdateProjectAsync(projectDto, userId!, isAdmin);

            return Ok();
        }

        [HttpDelete("delete/{id:guid}")]
        public async Task<IActionResult> DeleteProject([FromRoute] Guid id)
        {
            var isAdmin = User.IsInRole("Admin");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _manager.ProjectService.DeleteProjectAsync(id, userId!, isAdmin);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("restore/{id:guid}")]
        public async Task<IActionResult> RestoreProject([FromRoute] Guid id)
        {
            await _manager.ProjectService.RestoreProjectAsync(id);

            return Ok();
        }
    }
}
