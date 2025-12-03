using Entities.Dtos;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using Repositories.Extensions;

namespace Repositories
{
    public class TaskRepository : RepositoryBase<Entities.Models.Task>, ITaskRepository
    {
        public TaskRepository(RepositoryContext context) : base(context)
        {
        }

        // Task

        public async Task<IEnumerable<TaskDto>> GetProjectTasksAsync(string accountId, long projectId, TaskRequestParameters p, bool trackChanges)
        {
            var tasks = await FindByCondition(t => t.ProjectId == projectId, trackChanges)
                .FilterBy(p.Title, p => p.Title, FilterOperator.Contains)
                .FilterBy(p.Status, p => p.Status, FilterOperator.Equal)
                .FilterBy(p.Priority, p => p.Priority, FilterOperator.Equal)
                .FilterBy(p.LabelId, p => p.LabelId, FilterOperator.Equal)
                .FilterByMe(p.Me, accountId)
                .OrderBy(p => p.TaskSequence)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    CreatedByEmail = t.CreatedBy!.Email!,
                    AssignedToEmail = t.AssignedTo!.Email!,
                    Title = t.Title,
                    Status = t.Status,
                    Priority = t.Priority,
                    ProgressPercentage = t.ProgressPercentage,
                    CommentCount = t.Comments.Count,
                    AttachmentCount = t.Attachments.Count
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<Entities.Models.Task?> GetTaskByIdAsync(long projectId, Guid taskId, bool trackChanges)
        {
            var task = await FindByCondition(t => t.ProjectId == projectId && t.Id == taskId, trackChanges)
                .FirstOrDefaultAsync();

            return task;
        }

        public void CreateTask(Entities.Models.Task task)
        {
            Create(task);
        }

        public void UpdateTask(Entities.Models.Task task)
        {
            Update(task);
        }

        // Attachment

        public async Task<long> GetTaskIdAsync(long projectId, Guid taskId)
        {
            var id = await FindByCondition(t => t.ProjectId == projectId && t.Id == taskId, false)
                .Select(t => t.TaskSequence)
                .FirstOrDefaultAsync();

            return id;
        }

        public async Task<IEnumerable<TaskAttachmentDto>> GetTaskAttachmentsAsync(long projectId, Guid taskId, bool trackChanges)
        {
            var id = await GetTaskIdAsync(projectId, taskId);

            var attachments = await _context.TaskAttachments
                .Include(ta => ta.UploadedBy)
                .Where(ta => ta.Task!.ProjectId == projectId && ta.TaskId == id)
                .OrderBy(ta => ta.AttachmentSequence)
                .Select(ta => new TaskAttachmentDto
                {
                    Id = ta.Id,
                    FileUrl = ta.FileUrl,
                    ThumbnailUrl = ta.ThumbnailUrl,
                    UploadedByEmail = ta.UploadedBy!.Email!,
                    UploadedAt = ta.UploadedAt
                })
                .ToListAsync();

            return attachments;
        }

        public async Task<TaskAttachment?> GetTaskAttachmentByIdAsync(long projectId, Guid taskId, Guid attachmentId, bool trackChanges)
        {
            var id = await GetTaskIdAsync(projectId, taskId);

            var attachment = await _context.TaskAttachments
                .Where(ta => ta.Task!.ProjectId == projectId && ta.TaskId == id && ta.Id == attachmentId)
                .FirstOrDefaultAsync();
            return attachment;
        }

        public void CreateTaskAttachment(TaskAttachment attachment)
        {
            _context.TaskAttachments.Add(attachment);
        }

        public void UpdateTaskAttachment(TaskAttachment attachment)
        {
            _context.TaskAttachments.Update(attachment);
        }

        // TimeLog

        public async Task<IEnumerable<TimeLogDto>> GetTaskTimeLogsAsync(long projectId, Guid taskId, bool trackChanges)
        {
            var id = await GetTaskIdAsync(projectId, taskId);

            var timeLogs = await _context.TimeLogs
                .Include(tl => tl.LoggedBy)
                .Where(tl => tl.Task!.ProjectId == projectId && tl.TaskId == id)
                .OrderBy(tl => tl.TimeLogSequence)
                .Select(tl => new TimeLogDto
                {
                    Id = tl.Id,
                    LoggedByEmail = tl.LoggedBy!.Email!,
                    TimeLogCategoryName = tl.TimeLogCategory!.Name!,
                    TimeLogCategoryColor = tl.TimeLogCategory!.Color!,
                    Hours = tl.Hours
                })
                .ToListAsync();

            return timeLogs;
        }

        public async Task<TimeLog?> GetTimeLogByIdAsync(long projectId, Guid taskId, Guid logId, bool trackChanges)
        {
            var id = await GetTaskIdAsync(projectId, taskId);

            var timeLog = await _context.TimeLogs
                .Where(tl => tl.Task!.ProjectId == projectId && tl.TaskId == id && tl.Id == logId)
                .FirstOrDefaultAsync();

            return timeLog;
        }

        public void CreateTimeLog(TimeLog timeLog)
        {
            _context.TimeLogs.Add(timeLog);
        }

        public void UpdateTimeLog(TimeLog timeLog)
        {
            _context.TimeLogs.Update(timeLog);
        }

        // TimeLogCategory

        public async Task<IEnumerable<TimeLogCategoryDto>> GetProjectsTimeLogCategoriesAsync(long projectId, bool trackChanges)
        {
            var categories = await _context.TimeLogCategories
                .Where(tlc => tlc.ProjectId == projectId)
                .OrderBy(tlc => tlc.TimeLogCategorySequence)
                .Select(tlc => new TimeLogCategoryDto
                {
                    Id = tlc.Id,
                    Name = tlc.Name,
                    Color = tlc.Color
                })
                .ToListAsync();

            return categories;
        }

        public async Task<TimeLogCategory?> GetTimeLogCategoryByIdAsync(long projectId, Guid categoryId, bool trackChanges)
        {
            var category = await _context.TimeLogCategories
                .Where(tlc => tlc.ProjectId == projectId && tlc.Id == categoryId)
                .FirstOrDefaultAsync();

            return category;
        }

        public void CreateTimeLogCategory(TimeLogCategory timeLogCategory)
        {
            _context.TimeLogCategories.Add(timeLogCategory);
        }

        public void UpdateTimeLogCategory(TimeLogCategory timeLogCategory)
        {
            _context.TimeLogCategories.Update(timeLogCategory);
        }

        public void DeleteTimeLogCategory(TimeLogCategory timeLogCategory)
        {
            _context.TimeLogCategories.Remove(timeLogCategory);
        }
    }
}
