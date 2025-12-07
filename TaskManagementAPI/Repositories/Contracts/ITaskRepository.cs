using Entities.Dtos;
using Entities.Models;
using Entities.RequestFeatures;

namespace Repositories.Contracts
{
    public interface ITaskRepository : IRepositoryBase<Entities.Models.Task>
    {
        // Task
        Task<IEnumerable<TaskDto>> GetProjectTasksAsync(string accountId, long projectId, TaskRequestParameters p, bool trackChanges);
        Task<Entities.Models.Task?> GetTaskByIdAsync(long projectId, Guid taskId, bool forDelete, bool trackChanges);
        Task<long> GetTaskIdAsync(long projectId, Guid taskId);
        void CreateTask(Entities.Models.Task task);
        void UpdateTask(Entities.Models.Task task);

        // Attachment
        Task<IEnumerable<AttachmentDto>> GetTaskAttachmentsAsync(long projectId, Guid taskId, bool trackChanges);
        Task<Attachment?> GetTaskAttachmentByIdAsync(long projectId, Guid taskId, Guid attachmentId, bool trackChanges);
        void CreateTaskAttachment(Attachment attachment);
        void UpdateTaskAttachment(Attachment attachment);

        // TimeLog
        Task<IEnumerable<TimeLogDto>> GetTaskTimeLogsAsync(long projectId, Guid taskId, bool trackChanges);
        Task<TimeLog?> GetTimeLogByIdAsync(long projectId, Guid taskId, Guid logId, bool trackChanges);
        void CreateTimeLog(TimeLog timeLog);
        void UpdateTimeLog(TimeLog timeLog);

        // TimeLogCategory
        Task<IEnumerable<TimeLogCategoryDto>> GetProjectsTimeLogCategoriesAsync(long projectId, bool trackChanges);
        Task<TimeLogCategory?> GetTimeLogCategoryByIdAsync(long projectId, Guid categoryId, bool trackChanges);
        void CreateTimeLogCategory(TimeLogCategory timeLogCategory);
        void UpdateTimeLogCategory(TimeLogCategory timeLogCategory);
        void DeleteTimeLogCategory(TimeLogCategory timeLogCategory);
    }
}
