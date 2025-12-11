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
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Comments)
                .Include(t => t.Attachments)
                .Include(t => t.Label)
                .AsSplitQuery()
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
                    LabelId = t.Label!.Id,
                    Priority = t.Priority,
                    ProgressPercentage = t.ProgressPercentage,
                    CommentCount = t.Comments.Count,
                    AttachmentCount = t.Attachments.Count
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<Entities.Models.Task?> GetTaskByIdAsync(long projectId, Guid taskId, bool forDelete, bool trackChanges)
        {
            var taskQuery = FindByCondition(t => t.ProjectId == projectId && t.Id == taskId, trackChanges)
                .Include(t => t.CreatedBy)
                .Include(t => t.AssignedTo)
                .Include(t => t.Label);

            if (forDelete)
            {
                var taskForDelete = await taskQuery
                                .Include(t => t.Comments)
                                .Include(t => t.Attachments)
                                .Include(t => t.TimeLogs)
                                .FirstOrDefaultAsync();

                return taskForDelete;
            }

            var task = await taskQuery
                .FirstOrDefaultAsync();

            return task;
        }

        public async Task<long> GetTaskIdAsync(long projectId, Guid taskId)
        {
            var taskSequence = await FindByCondition(t => t.ProjectId == projectId && t.Id == taskId, false)
                .Select(t => t.TaskSequence)
                .FirstOrDefaultAsync();

            return taskSequence;
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

        public async Task<IEnumerable<AttachmentDto>> GetTaskAttachmentsAsync(long projectId, Guid taskId, bool trackChanges)
        {
            var attachmentsQuery = _context.Attachments
                .Where(ta => ta.Task!.ProjectId == projectId && ta.Task.Id == taskId)
                .OrderBy(ta => ta.AttachmentSequence)
                .Select(ta => new AttachmentDto
                {
                    Id = ta.Id,
                    FileUrl = ta.FileUrl,
                    ThumbnailUrl = ta.ThumbnailUrl,
                    UploadedByEmail = ta.UploadedBy!.Email!,
                    UploadedAt = ta.UploadedAt
                });

            return await (trackChanges ? attachmentsQuery : attachmentsQuery.AsNoTracking())
                .ToListAsync();
        }

        public async Task<Attachment?> GetTaskAttachmentByIdAsync(long projectId, Guid taskId, Guid attachmentId, bool trackChanges)
        {
            var attachmentQuery = _context.Attachments
                .Include(ta => ta.UploadedBy)
                .Where(ta => ta.Task!.ProjectId == projectId && ta.Task.Id == taskId && ta.Id == attachmentId);

            return await (trackChanges ? attachmentQuery : attachmentQuery.AsNoTracking())
                .FirstOrDefaultAsync();
        }

        public void CreateTaskAttachment(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
        }

        public void UpdateTaskAttachment(Attachment attachment)
        {
            _context.Attachments.Update(attachment);
        }

        // TimeLog

        public async Task<IEnumerable<TimeLogDto>> GetTaskTimeLogsAsync(long projectId, Guid taskId, bool trackChanges)
        {
            var timeLogsQuery = _context.TimeLogs
                .Where(tl => tl.Task!.ProjectId == projectId && tl.Task.Id == taskId)
                .OrderBy(tl => tl.TimeLogSequence)
                .Select(tl => new TimeLogDto
                {
                    Id = tl.Id,
                    LoggedByEmail = tl.LoggedBy!.Email!,
                    TimeLogCategoryName = tl.TimeLogCategory!.Name!,
                    TimeLogCategoryColor = tl.TimeLogCategory!.Color!,
                    Hours = tl.Hours
                });

            return await (trackChanges ? timeLogsQuery : timeLogsQuery.AsNoTracking())
                .ToListAsync();
        }

        public async Task<TimeLog?> GetTimeLogByIdAsync(long projectId, Guid taskId, Guid logId, bool trackChanges)
        {
            var timeLogQuery = _context.TimeLogs
                .Include(tl => tl.LoggedBy)
                .Include(tl => tl.TimeLogCategory)
                .Where(tl => tl.Task!.ProjectId == projectId && tl.Task.Id == taskId && tl.Id == logId);

            return await (trackChanges ? timeLogQuery : timeLogQuery.AsNoTracking())
                .FirstOrDefaultAsync();
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
            var categoriesQuery = _context.TimeLogCategories
                .Where(tlc => tlc.ProjectId == projectId)
                .OrderBy(tlc => tlc.TimeLogCategorySequence)
                .Select(tlc => new TimeLogCategoryDto
                {
                    Id = tlc.Id,
                    Name = tlc.Name,
                    Color = tlc.Color
                });

            return await (trackChanges ? categoriesQuery : categoriesQuery.AsNoTracking())
                .ToListAsync();
        }

        public async Task<TimeLogCategory?> GetTimeLogCategoryByIdAsync(long projectId, Guid categoryId, bool trackChanges)
        {
            var categoryQuery = _context.TimeLogCategories
                .Include(tlc => tlc.TimeLogs)
                    .ThenInclude(tl => tl.LoggedBy)
                .Where(tlc => tlc.ProjectId == projectId && tlc.Id == categoryId);

            return await (trackChanges ? categoryQuery : categoryQuery.AsNoTracking())
                .FirstOrDefaultAsync();
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
