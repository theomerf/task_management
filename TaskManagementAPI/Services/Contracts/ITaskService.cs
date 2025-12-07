using Entities.Dtos;
using Entities.Models;
using Entities.RequestFeatures;

namespace Services.Contracts
{
    public interface ITaskService
    {
        // Task
        Task<IEnumerable<TaskDto>> GetProjectTasksAsync(Guid projectId, TaskRequestParameters p, string accountId, bool isAdmin);
        Task<TaskDetailsDto> GetTaskByIdAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin);
        System.Threading.Tasks.Task CreateTaskAsync(Guid projectId, TaskDtoForCreation taskDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task UpdateTaskAsync(Guid projectId, TaskDtoForUpdate taskDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task UpdateTaskStatusAsync(Guid projectId, TaskDtoForStatusUpdate taskDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task UpdateTaskPriorityAsync(Guid projectId, TaskDtoForPriorityUpdate taskDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task DeleteTaskAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin);

        // Attachment
        Task<IEnumerable<AttachmentDto>> GetTaskAttachmentsAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin);
        Task<AttachmentDetailsDto> GetTaskAttachmentByIdAsync(Guid projectId, Guid taskId, Guid attachmentId, string accountId, bool isAdmin);
        System.Threading.Tasks.Task CreateTaskAttachmentAsync(Guid projectId, AttachmentDtoForCreation attachmentDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task UpdateTaskAttachmentAsync(Guid projectId, AttachmentDtoForUpdate attachmentDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task DeleteTaskAttachmentAsync(Guid projectId, Guid taskId, Guid attachmentId, string accountId, bool isAdmin);

        // TimeLog
        Task<IEnumerable<TimeLogDto>> GetTaskTimeLogsAsync(Guid projectId, Guid taskId, string accountId, bool isAdmin);
        Task<TimeLogDetailsDto> GetTimeLogByIdAsync(Guid projectId, Guid taskId, Guid logId, string accountId, bool isAdmin);
        System.Threading.Tasks.Task CreateTimeLogAsync(Guid projectId, TimeLogDtoForCreation timeLogDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task UpdateTimeLogAsync(Guid projectId, TimeLogDtoForUpdate timeLogDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task DeleteTimeLogAsync(Guid projectId, Guid taskId, Guid logId, string accountId, bool isAdmin);

        // TimeLogCategory
        Task<IEnumerable<TimeLogCategoryDto>> GetProjectsTimeLogCategoriesAsync(Guid projectId, string accountId, bool isAdmin);
        Task<TimeLogCategoryDetailsDto> GetTimeLogCategoryByIdAsync(Guid projectId, Guid categoryId, string accountId, bool isAdmin);
        System.Threading.Tasks.Task CreateTimeLogCategoryAsync(Guid projectId, TimeLogCategoryDtoForCreation categoryDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task UpdateTimeLogCategoryAsync(Guid projectId, TimeLogCategoryDtoForUpdate categoryDto, string accountId, bool isAdmin);
        System.Threading.Tasks.Task DeleteTimeLogCategoryAsync(Guid projectId, Guid categoryId, string accountId, bool isAdmin);
    }
}
