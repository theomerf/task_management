using AutoMapper;
using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class TaskManager : ITaskService
    {
        private readonly IRepositoryManager _manager;
        private readonly IMapper _mapper;

        public TaskManager(IRepositoryManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        private async Task<long> TaskAuthorizationCheckAsync(string accountId, Guid projectId, string errorMessage, bool isAdmin, bool isManage)
        {
            var (isAuthorized, id) = isManage ?
                  await _manager.Authorization.CanAccessTasksAsync(accountId, projectId, isAdmin)
                : await _manager.Authorization.CanManageTasksAsync(accountId, projectId, isAdmin);

            if (!isAuthorized)
                throw new AccessViolationException(errorMessage);

            return id!.Value;
        }

        // Task
        public async Task<IEnumerable<TaskDto>> GetProjectTasksAsync(Guid projectId, TaskRequestParameters p, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu proje görevlerine erişim yetkiniz bulunmamaktadır.", isAdmin, false);
            var tasks = await _manager.Task.GetProjectTasksAsync(accountId, id, p, false);

            return tasks;
        }

        private async Task<Entities.Models.Task> GetTaskByIdForServiceAsync(long projectId, Guid taskId, bool trackChanges)
        {
            var task = await _manager.Task.GetTaskByIdAsync(projectId, taskId, trackChanges);

            if (task == null)
                throw new TaskNotFoundException(taskId);

            return task;
        }
        public async Task<TaskDetailsDto> GetTaskByIdAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu göreve erişim yetkiniz bulunmamaktadır.", isAdmin, false);

            var task = await GetTaskByIdForServiceAsync(id, taskId, false);
            var taskDto = _mapper.Map<TaskDetailsDto>(task);

            return taskDto;
        }

        public async System.Threading.Tasks.Task CreateTaskAsync(Guid projectId, TaskDtoForCreation taskDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev oluşturma yetkiniz bulunmamaktadır.", isAdmin, true);

            var task = _mapper.Map<Entities.Models.Task>(taskDto);
            task.ProjectId = id;
            task.CreatedById = accountId;

            if (taskDto.LabelId != null)
            {
                var label = await _manager.Project.GetLabelByIdAsync(id, taskDto.LabelId.Value, false);

                if (label != null)
                    task.LabelId = label.LabelSequence;
            }
            _manager.Task.CreateTask(task);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateTaskAsync(Guid projectId, TaskDtoForUpdate taskDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev düzenleme yetkiniz bulunmamaktadır.", isAdmin, true);

            var task = await GetTaskByIdForServiceAsync(id, taskDto.Id, true);
            _mapper.Map(taskDto, task);
            if (taskDto.LabelId != null)
            {
                var label = await _manager.Project.GetLabelByIdAsync(id, taskDto.LabelId.Value, false);

                if (label != null)
                    task.LabelId = label.LabelSequence;
            }
            task.UpdatedAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateTaskStatusAsync(Guid projectId, TaskDtoForStatusUpdate taskDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev düzenleme yetkiniz bulunmamaktadır.", isAdmin, true);

            var task = await GetTaskByIdForServiceAsync(id, taskDto.Id, true);
            _mapper.Map(taskDto, task);

            task.UpdatedAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateTaskPriorityAsync(Guid projectId, TaskDtoForPriorityUpdate taskDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev düzenleme yetkiniz bulunmamaktadır.", isAdmin, true);

            var task = await GetTaskByIdForServiceAsync(id, taskDto.Id, true);
            _mapper.Map(taskDto, task);

            task.UpdatedAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev silme yetkiniz bulunmamaktadır.", isAdmin, true);

            var task = await GetTaskByIdForServiceAsync(id, taskId, true);
            task.DeletedAt = DateTime.UtcNow;
            
            await _manager.SaveAsync();
        }

        // Attachment
        public async Task<IEnumerable<TaskAttachmentDto>> GetTaskAttachmentsAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev eklerini görme yetkiniz bulunmamaktadır.", isAdmin, false);
            var attachments = await _manager.Task.GetTaskAttachmentsAsync(id, taskId, false);

            return attachments;
        }

        public async Task<TaskAttachment> GetTaskAttachmentByIdForServiceAsync(long projectId, Guid taskId, Guid attachmentId, bool trackChanges)
        {
            var attachment = await _manager.Task.GetTaskAttachmentByIdAsync(projectId, taskId, attachmentId, trackChanges);

            if (attachment == null)
                throw new TaskAttachmentNotFoundException(attachmentId);

            return attachment;
        }

        public async Task<TaskAttachmentDetailsDto> GetTaskAttachmentByIdAsync(Guid projectId, Guid taskId, Guid attachmentId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu görev ekini görme yetkiniz bulunmamaktadır.", isAdmin, false);

            var attachment = await GetTaskAttachmentByIdForServiceAsync(id, taskId, attachmentId, false);
            var attachmentDto = _mapper.Map<TaskAttachmentDetailsDto>(attachment);

            return attachmentDto;
        }

        public async System.Threading.Tasks.Task CreateTaskAttachmentAsync(Guid projectId, TaskAttachmentDtoForCreation attachmentDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev eki oluşturma yetkiniz bulunmamaktadır.", isAdmin, true);
            var taskId = await _manager.Task.GetTaskIdAsync(id, attachmentDto.TaskId);

            var attachment = _mapper.Map<TaskAttachment>(attachmentDto);
            attachment.TaskId = taskId;
            _manager.Task.CreateTaskAttachment(attachment);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateTaskAttachmentAsync(Guid projectId, TaskAttachmentDtoForUpdate attachmentDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev eki düzenleme yetkiniz bulunmamaktadır.", isAdmin, true);

            var attachment = await GetTaskAttachmentByIdForServiceAsync(id, attachmentDto.TaskId, attachmentDto.Id, true);
            _mapper.Map(attachmentDto, attachment);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task DeleteTaskAttachmentAsync(Guid projectId, Guid taskId, Guid attachmentId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev eki silme yetkiniz bulunmamaktadır.", isAdmin, true);

            var attachment = await GetTaskAttachmentByIdForServiceAsync(id, taskId, attachmentId, true);
            attachment.DeletedAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        // TimeLog
        public async Task<IEnumerable<TimeLogDto>> GetTaskTimeLogsAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede görev zaman raporlarını görme yetkiniz bulunmamaktadır.", isAdmin, false);
            var timeLogs = await _manager.Task.GetTaskTimeLogsAsync(id, taskId, false);

            return timeLogs;
        }

        public async Task<TimeLog> GetTimeLogByIdForServiceAsync(long projectId, Guid taskId, Guid logId, bool trackChanges)
        {
            var timeLog = await _manager.Task.GetTimeLogByIdAsync(projectId, taskId, logId, trackChanges);

            if (timeLog == null)
                throw new TimeLogNotFoundException(logId);

            return timeLog;
        }

        public async Task<TimeLogDetailsDto> GetTimeLogByIdAsync(Guid projectId, Guid taskId, Guid logId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu görev zaman raporunu görme yetkiniz bulunmamaktadır.", isAdmin, false);

            var timeLog = await GetTimeLogByIdForServiceAsync(id, taskId, logId, false);
            var timeLogDto = _mapper.Map<TimeLogDetailsDto>(timeLog);

            return timeLogDto;
        }

        public async System.Threading.Tasks.Task CreateTimeLogAsync(Guid projectId, TimeLogDtoForCreation timeLogDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede zaman raporunu oluşturma yetkiniz bulunmamaktadır.", isAdmin, true);
            var taskId = await _manager.Task.GetTaskIdAsync(id, timeLogDto.TaskId);

            var timeLog = _mapper.Map<TimeLog>(timeLogDto);
            timeLog.LoggedById = accountId;
            timeLog.TaskId = taskId;

            if (timeLogDto.TimeLogCategoryId != null)
            {
                var timeLogCategory = await GetTimeLogCategoryByIdForServiceAsync(id, timeLogDto.TimeLogCategoryId.Value, false);
                timeLog.TimeLogCategoryId = timeLogCategory.TimeLogCategorySequence;
            }
            _manager.Task.CreateTimeLog(timeLog);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateTimeLogAsync(Guid projectId, TimeLogDtoForUpdate timeLogDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede zaman raporu düzenleme yetkiniz bulunmamaktadır.", isAdmin, true);

            var timeLog = await GetTimeLogByIdForServiceAsync(id, timeLogDto.TaskId, timeLogDto.Id, true);
            if (timeLogDto.TimeLogCategoryId != null)
            {
                var timeLogCategory = await GetTimeLogCategoryByIdForServiceAsync(id, timeLogDto.TimeLogCategoryId.Value, false);
                timeLog.TimeLogCategoryId = timeLogCategory.TimeLogCategorySequence;
            }
            _mapper.Map(timeLogDto, timeLog);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task DeleteTimeLogAsync(Guid projectId, Guid taskId, Guid logId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede zaman raporu silme yetkiniz bulunmamaktadır.", isAdmin, true);

            var timeLog = await GetTimeLogByIdForServiceAsync(id, taskId, logId, true);
            timeLog.DeletedAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        // TimeLogCategory
        public async Task<IEnumerable<TimeLogCategoryDto>> GetProjectsTimeLogCategoriesAsync(Guid projectId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede zaman raporu kategorilerini görme yetkiniz bulunmamaktadır.", isAdmin, false);
            var timeLogCategories = await _manager.Task.GetProjectsTimeLogCategoriesAsync(id, false);

            return timeLogCategories;
        }

        private async Task<TimeLogCategory> GetTimeLogCategoryByIdForServiceAsync(long projectId, Guid categoryId, bool trackChanges)
        {
            var timeLogCategory = await _manager.Task.GetTimeLogCategoryByIdAsync(projectId, categoryId, trackChanges);

            if (timeLogCategory == null)
                throw new TimeLogCategoryNotFoundException(categoryId);

            return timeLogCategory;
        }

        public async Task<TimeLogCategoryDetailsDto> GetTimeLogCategoryByIdAsync(Guid projectId, Guid categoryId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu zaman raporu kategorisini görme yetkiniz bulunmamaktadır.", isAdmin, false);

            var timeLogCategory = await GetTimeLogCategoryByIdForServiceAsync(id, categoryId, false);
            var timeLogCategoryDto = _mapper.Map<TimeLogCategoryDetailsDto>(timeLogCategory);

            return timeLogCategoryDto;
        }

        public async System.Threading.Tasks.Task CreateTimeLogCategoryAsync(Guid projectId, TimeLogCategoryDtoForCreation categoryDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede zaman raporu kategorisi oluşturma yetkiniz bulunmamaktadır.", isAdmin, true);

            var timeLogCategory = _mapper.Map<TimeLogCategory>(categoryDto);
            timeLogCategory.ProjectId = id;
            _manager.Task.CreateTimeLogCategory(timeLogCategory);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task UpdateTimeLogCategoryAsync(Guid projectId, TimeLogCategoryDtoForUpdate categoryDto, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede zaman raporu kategorisi düzenleme yetkiniz bulunmamaktadır.", isAdmin, true);

            var timeLogCategory = await GetTimeLogCategoryByIdForServiceAsync(id, categoryDto.Id, true);
            _mapper.Map(categoryDto, timeLogCategory);

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task DeleteTimeLogCategoryAsync(Guid projectId, Guid categoryId, string accountId, bool isAdmin)
        {
            var id = await TaskAuthorizationCheckAsync(accountId, projectId, "Bu projede zaman raporu kategorisi silme yetkiniz bulunmamaktadır.", isAdmin, true);

            var timeLogCategory = await GetTimeLogCategoryByIdForServiceAsync(id, categoryId, false);
            _manager.Task.DeleteTimeLogCategory(timeLogCategory);

            await _manager.SaveAsync();
        }
    }
}
