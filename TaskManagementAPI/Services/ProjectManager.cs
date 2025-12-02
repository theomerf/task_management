using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class ProjectManager : IProjectService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public ProjectManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        public async Task<(PagedList<ProjectDto> pagedProjects, MetaData metaData)> GetAllProjectsForAdminAsync(ProjectRequestParametersForAdmin p, bool trackChanges)
        {
            var projects = await _manager.Project.GetAllProjectsForAdminAsync(p, trackChanges);

            var pagedProducts = PagedList<ProjectDto>.ToPagedList(projects.projects, p.PageNumber, p.PageSize, projects.totalCount);

            return (pagedProducts, pagedProducts.MetaData);
        }

        public async Task<IEnumerable<ProjectDto>> GetUsersProjectsAsync(string accountId, ProjectRequestParameters p)
        {
            var projects = await _manager.Project.GetUsersProjectsAsync(accountId, p, false);

            return projects;
        }

        private async Task<Project> GetProjectByIdForServiceAsync(Guid projectId, bool trackChanges)
        {
            var project = await _manager.Project.GetProjectByIdAsync(projectId, trackChanges);

            if (project == null) 
            {
                throw new ProjectNotFoundException(projectId);
            }

            return project;
        }

        public async Task<ProjectDetailsDto> GetProjectByIdAsync(Guid projectId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false);

            if (isAdmin != true)
            {
                bool canAccess = await _manager.Authorization.CanAccessProjectAsync(accountId!, project);

                if (!canAccess)
                    throw new AccessViolationException("Bu projeye erişim izniniz yok.");
            }
                
            var projectDto = _mapper.Map<ProjectDetailsDto>(project);

            return projectDto;
        }

        public async System.Threading.Tasks.Task CreateProjectAsync(ProjectDtoForCreation projectDto, bool canCreate)
        {
            if (canCreate)
            {
                var project = _mapper.Map<Project>(projectDto);

                _manager.Project.CreateProject(project);

                await _manager.SaveAsync();
            }
            else
            {
                throw new AccessViolationException("Yeni proje oluşturma izniniz yok.");
            }
        }

        public async System.Threading.Tasks.Task DeleteProjectAsync(Guid projectId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, true);

            if (!isAdmin)
            {
                var canDelete = await _manager.Authorization.CanDeleteProjectAsync(accountId!, project);

                if (!canDelete)
                {
                    throw new AccessViolationException("Bu projeyi silme izniniz yok.");
                }
            }

            project.DeletedAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task RestoreProjectAsync(Guid projectId)
        {
            var project = await _manager.Project.GetProjectByIdForRestoreAsync(projectId, true);

            if (project == null)
            {
                throw new ProjectNotFoundException(projectId);
            }
            project.DeletedAt = null;

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateProjectAsync(ProjectDtoForUpdate projectDto, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectDto.Id, true);

            if (!isAdmin)
            {
                var canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);

                if (!canManage)
                    throw new AccessViolationException("Bu projeyi düzenleme izniniz yok.");
            }

            _mapper.Map(projectDto, project);

            await _manager.SaveAsync();
        }
    }
}
