using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Identity;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class ProjectManager : IProjectService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;

        public ProjectManager(IRepositoryManager manager, IMapper mapper, UserManager<Account> userManager)
        {
            _manager = manager;
            _mapper = mapper;
            _userManager = userManager;
        }

        // Project
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

        private async Task<Project> GetProjectByIdForServiceAsync(Guid projectId, bool forDelete, bool trackChanges)
        {
            var project = await _manager.Project.GetProjectByIdAsync(projectId, forDelete, trackChanges);

            if (project == null)
                throw new ProjectNotFoundException(projectId);

            return project;
        }

        public async Task<ProjectDetailsDto> GetProjectByIdAsync(Guid projectId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (isAdmin != true)
            {
                bool canAccess = await _manager.Authorization.CanAccessProjectAsync(accountId!, project);

                if (!canAccess)
                    throw new ForbiddenException("Bu projeye erişim izniniz yok.");
            }

            var projectDto = _mapper.Map<ProjectDetailsDto>(project);

            return projectDto;
        }

        public async System.Threading.Tasks.Task CreateProjectAsync(ProjectDtoForCreation projectDto, string accountId, bool canCreate)
        {
            if (!canCreate)
                throw new ForbiddenException("Yeni proje oluşturma izniniz yok.");

            var project = _mapper.Map<Project>(projectDto);
            project.CreatedById = accountId;
            _manager.Project.CreateProject(project);

            await _manager.SaveAsync();

            var activityLog = new ActivityLog
            {
                PerformedById = accountId,
                RelatedProjectId = project.ProjectSequence,
                Type = ActivityType.ProjectCreated,
                Description = "Proje oluşturuldu.",
                CreatedAt = DateTime.UtcNow
            };

            _manager.ActivityLog.CreateActivityLog(activityLog);
            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task DeleteProjectAsync(Guid projectId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, true, true);

            if (!isAdmin)
            {
                var canDelete = await _manager.Authorization.CanDeleteProjectAsync(accountId!, project);

                if (!canDelete)
                {
                    throw new ForbiddenException("Bu projeyi silme izniniz yok.");
                }
            }

            project.DeletedAt = DateTime.UtcNow;
            var tasks = project.Tasks;
            foreach (var task in tasks)
            {
                task.DeletedAt = DateTime.UtcNow;
                foreach (var timeLog in task.TimeLogs)
                {
                    timeLog.DeletedAt = DateTime.UtcNow;
                }
                foreach (var comment in task.Comments)
                {
                    comment.DeletedAt = DateTime.UtcNow;
                }
                foreach (var attachment in task.Attachments)
                {
                    attachment.DeletedAt = DateTime.UtcNow;
                }
            }

            var activityLog = new ActivityLog
            {
                PerformedById = accountId,
                RelatedProjectId = project.ProjectSequence,
                Type = ActivityType.ProjectDeleted,
                Description = "Proje silindi.",
                CreatedAt = DateTime.UtcNow
            };

            _manager.ActivityLog.CreateActivityLog(activityLog);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task RestoreProjectAsync(Guid projectId)
        {
            var project = await _manager.Project.GetProjectByIdForRestoreAsync(projectId, true);

            if (project == null)
                throw new ProjectNotFoundException(projectId);

            project.DeletedAt = null;
            var tasks = project.Tasks;
            foreach (var task in tasks)
            {
                task.DeletedAt = null;
                foreach (var timeLog in task.TimeLogs)
                {
                    timeLog.DeletedAt = null;
                }
                foreach (var comment in task.Comments)
                {
                    comment.DeletedAt = null;
                }
                foreach (var attachment in task.Attachments)
                {
                    attachment.DeletedAt = null;
                }
            }

            var activityLog = new ActivityLog
            {
                PerformedById = null,
                RelatedProjectId = project.ProjectSequence,
                Type = ActivityType.ProjectRestored,
                Description = "Proje geri yüklendi.",
                CreatedAt = DateTime.UtcNow
            };
            _manager.ActivityLog.CreateActivityLog(activityLog);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateProjectAsync(ProjectDtoForUpdate projectDto, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectDto.Id, false, true);

            if (!isAdmin)
            {
                var canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);

                if (!canManage)
                    throw new ForbiddenException("Bu projeyi düzenleme izniniz yok.");
            }

            if (project.Status != projectDto.Status)
            {
                if (projectDto.Status == ProjectStatus.Completed)
                {
                    var completedActivityLog = new ActivityLog
                    {
                        PerformedById = accountId,
                        RelatedProjectId = project.ProjectSequence,
                        Type = ActivityType.ProjectCompleted,
                        Description = "Proje tamamlandı.",
                        CreatedAt = DateTime.UtcNow
                    };

                    _manager.ActivityLog.CreateActivityLog(completedActivityLog);
                }
                else if (projectDto.Status == ProjectStatus.Archived)
                {
                    var archivedActivityLog = new ActivityLog
                    {
                        PerformedById = accountId,
                        RelatedProjectId = project.ProjectSequence,
                        Type = ActivityType.ProjectArchived,
                        Description = "Proje arşivlendi.",
                        CreatedAt = DateTime.UtcNow
                    };

                    _manager.ActivityLog.CreateActivityLog(archivedActivityLog);
                }
            }

            _mapper.Map(projectDto, project);
            project.CreatedById = accountId;
            project.UpdatedAt = DateTime.UtcNow;
            project.Settings.ProjectId = project.ProjectSequence;

            var activityLog = new ActivityLog
            {
                PerformedById = accountId,
                RelatedProjectId = project.ProjectSequence,
                Type = ActivityType.ProjectUpdated,
                Description = "Proje güncellendi.",
                CreatedAt = DateTime.UtcNow
            };

            _manager.ActivityLog.CreateActivityLog(activityLog);

            await _manager.SaveAsync();
        }

        // Settings
        private async Task<ProjectSetting> GetProjectSettingsByIdForServiceAsync(long projectSequence, Guid projectId, bool trackChanges)
        {
            var settings = await _manager.Project.GetProjectSettingsAsync(projectSequence, trackChanges);

            if (settings == null)
                throw new ProjectSettingNotFoundException(projectId);

            return settings;
        }
        public async Task<ProjectSettingDto> GetProjectSettingsAsync(Guid projectId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canAccess = await _manager.Authorization.CanAccessProjectAsync(accountId!, project);
                if (!canAccess)
                    throw new ForbiddenException("Bu proje ayarlarına erişim izniniz yok.");
            }

            var settings = await GetProjectSettingsByIdForServiceAsync(project.ProjectSequence, projectId, false);
            var settingsDto = _mapper.Map<ProjectSettingDto>(settings);

            return settingsDto;
        }

        public async System.Threading.Tasks.Task UpdateProjectSettingsAsync(Guid projectId, ProjectSettingDtoForUpdate settingsDto, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);
                if (!canManage)
                    throw new ForbiddenException("Bu proje ayarlarını düzenleme izniniz yok.");
            }

            var settings = await GetProjectSettingsByIdForServiceAsync(project.ProjectSequence, settingsDto.Id, true);
            _mapper.Map(settingsDto, settings);
            settings.ProjectId = project.ProjectSequence;
            settings.UpdatedAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        // Label
        public async Task<IEnumerable<LabelDto>> GetProjectLabelsAsync(Guid projectId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canAccess = await _manager.Authorization.CanAccessProjectAsync(accountId!, project);

                if (!canAccess)
                    throw new ForbiddenException("Bu projeye erişim izniniz yok.");
            }

            var labels = await _manager.Project.GetProjectLabelsAsync(project.ProjectSequence, false);
            var labelsDto = _mapper.Map<IEnumerable<LabelDto>>(labels);

            return labelsDto;
        }

        private async Task<Label> GetLabelByIdForServiceAsync(long projectSequence, Guid labelId, bool trackChanges)
        {
            var label = await _manager.Project.GetLabelByIdAsync(projectSequence, labelId, trackChanges);

            if (label == null)
                throw new LabelNotFoundException(labelId);

            return label;
        }

        public async Task<LabelDto> GetLabelByIdAsync(Guid projectId, Guid labelId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canAccess = await _manager.Authorization.CanAccessProjectAsync(accountId!, project);

                if (!canAccess)
                    throw new ForbiddenException("Bu proje etiketlerine erişim izniniz yok.");
            }

            var label = await GetLabelByIdForServiceAsync(project.ProjectSequence, labelId, false);

            var labelDto = _mapper.Map<LabelDto>(label);
            return labelDto;
        }

        public async System.Threading.Tasks.Task CreateLabelAsync(Guid projectId, LabelDtoForCreation labelDto, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);

                if (!canManage)
                    throw new ForbiddenException("Bu projede etiket oluşturma izniniz yok.");
            }

            var label = _mapper.Map<Label>(labelDto);
            label.ProjectId = project.ProjectSequence;

            _manager.Project.CreateLabel(label);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateLabelAsync(Guid projectId, LabelDtoForUpdate labelDto, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);

                if (!canManage)
                    throw new ForbiddenException("Bu projede etiket düzenleme izniniz yok.");
            }

            var label = await GetLabelByIdForServiceAsync(project.ProjectSequence, labelDto.Id, true);
            _mapper.Map(labelDto, label);
            label.ProjectId = project.ProjectSequence;

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task DeleteLabelAsync(Guid projectId, Guid labelId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);

                if (!canManage)
                    throw new ForbiddenException("Bu projede etiket silme izniniz yok.");
            }

            var label = await GetLabelByIdForServiceAsync(project.ProjectSequence, labelId, true);
            _manager.Project.DeleteLabel(label);

            await _manager.SaveAsync();
        }

        // Member
        public async Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(Guid projectId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canAccess = await _manager.Authorization.CanAccessProjectAsync(accountId!, project);

                if (!canAccess)
                    throw new ForbiddenException("Bu proje üyelerine erişim izniniz yok.");
            }

            var members = await _manager.Project.GetProjectMembersAsync(project.ProjectSequence, false);
            var membersDto = _mapper.Map<IEnumerable<ProjectMemberDto>>(members);

            return membersDto;
        }

        private async Task<ProjectMember> GetProjectMemberByIdForServiceAsync(long projectSequence, Guid memberId, bool trackChanges)
        {
            var member = await _manager.Project.GetProjectMemberByIdAsync(projectSequence, memberId, trackChanges);

            if (member == null)
                throw new ProjectMemberNotFoundException(memberId);

            return member;
        }

        public async Task<ProjectMemberDto> GetProjectMemberByIdAsync(Guid projectId, Guid memberId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canAccess = await _manager.Authorization.CanAccessProjectAsync(accountId!, project);
                if (!canAccess)
                    throw new ForbiddenException("Bu proje üyesine erişim izniniz yok.");
            }

            var member = await GetProjectMemberByIdForServiceAsync(project.ProjectSequence, memberId, false);
            var memberDto = _mapper.Map<ProjectMemberDto>(member);

            return memberDto;
        }

        public async System.Threading.Tasks.Task AddMemberAsync(Guid projectId, ProjectMemberDtoForCreation memberDto, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, false);

            if (!isAdmin)
            {
                bool canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);

                if (!canManage)
                    throw new ForbiddenException("Bu projeye üye ekleme izniniz yok.");
            }
            
            var account = await _userManager.FindByEmailAsync(memberDto.Email);

            if (account == null)
                throw new AccountNotFoundException(memberDto.Email);

            var existingMember = await _manager.Project.GetMemberByAccountIdAsync(project.ProjectSequence, account.Id);

            if (existingMember != null && existingMember.LeftAt == null)
                throw new InvalidOperationException("Bu kullanıcı zaten projede aktif bir üye olarak bulunmaktadır.");

            if (existingMember != null && existingMember.LeftAt != null)
            {
                existingMember.LeftAt = null;
                existingMember.Role = memberDto.Role;

                await _manager.SaveAsync();
                return;
            }

            var activityLog = new ActivityLog
            {
                PerformedById = accountId,
                RelatedProjectId = project.ProjectSequence,
                Type = ActivityType.MemberAdded,
                Description = $"Proje üyesi eklendi: {account.Email}",
                CreatedAt = DateTime.UtcNow
            };
            _manager.ActivityLog.CreateActivityLog(activityLog);

            var notification = new Notification
            {
                RecipientId = account.Id,
                InitiatorId = accountId,
                RelatedProjectId = project.ProjectSequence,
                Type = NotificationType.ProjectInvitation,
                Title = "Projeye davet edildiniz",
                Message = $"'{project.Name}' projesine üye daveti aldınız.",
                CreatedAt = DateTime.UtcNow
            };
            _manager.Notification.CreateNotification(notification);

            var member = _mapper.Map<ProjectMember>(memberDto);
            member.ProjectId = project.ProjectSequence;
            member.AccountId = account.Id;
            _manager.Project.CreateProjectMember(member);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateMemberAsync(Guid projectId, ProjectMemberDtoForUpdate memberDto, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, true);

            if (!isAdmin)
            {
                bool canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);

                if (!canManage)
                    throw new ForbiddenException("Bu proje üyesini düzenleme izniniz yok.");
            }

            var member = await GetProjectMemberByIdForServiceAsync(project.ProjectSequence, memberDto.Id, true);

            if (memberDto.Role != memberDto.Role)
            {
                var account = await _userManager.FindByIdAsync(member.AccountId!);
                var activityLog = new ActivityLog
                {
                    PerformedById = accountId,
                    RelatedProjectId = project.ProjectSequence,
                    Type = ActivityType.MemberRoleChanged,
                    Description = $"Proje üyesi rolü güncellendi: {account!.Email}",
                    OldValue = member.Role.ToString(),
                    NewValue = memberDto.Role.ToString(),
                    CreatedAt = DateTime.UtcNow
                };

                _manager.ActivityLog.CreateActivityLog(activityLog);
            }

            _mapper.Map(memberDto, member);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task RemoveMemberAsync(Guid projectId, Guid memberId, string accountId, bool isAdmin)
        {
            var project = await GetProjectByIdForServiceAsync(projectId, false, true);

            if (!isAdmin)
            {
                bool canManage = await _manager.Authorization.CanManageProjectAsync(accountId!, project);

                if (!canManage)
                    throw new ForbiddenException("Bu proje üyesini silme izniniz yok.");
            }

            var member = await GetProjectMemberByIdForServiceAsync(project.ProjectSequence, memberId, true);
            member.LeftAt = DateTime.UtcNow;

            var account = await _userManager.FindByIdAsync(member.AccountId!);

            var activityLog = new ActivityLog
            {
                PerformedById = accountId,
                RelatedProjectId = project.ProjectSequence,
                Type = ActivityType.MemberRemoved,
                Description = $"Proje üyesi kaldırıldı: {account!.Email}",
                CreatedAt = DateTime.UtcNow
            };
            _manager.ActivityLog.CreateActivityLog(activityLog);

            await _manager.SaveAsync();
        }
    }
}
