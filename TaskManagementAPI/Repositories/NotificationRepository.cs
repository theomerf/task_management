using Entities.Dtos;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        public NotificationRepository(RepositoryContext context) : base(context)
        {
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId, bool trackChanges)
        {
            var notifications = await FindByCondition(n => n.RecipientId == userId && n.IsArchived == false, trackChanges)
                .Select(n => new NotificationDto
                {
                    Id = n.Id,
                    RelatedTaskTitle = n.RelatedTask != null ? n.RelatedTask.Title : string.Empty,
                    RelatedProjectName = n.RelatedProject != null ? n.RelatedProject.Name : string.Empty,
                    Type = n.Type,
                    Title = n.Title,
                    Message = n.Message,
                    Icon = n.Icon,
                    IsRead = n.IsRead,
                    IsArchived = n.IsArchived,
                    CreatedAt = n.CreatedAt
                })
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return notifications;
        }

        public async Task<Notification?> GetNotificationByIdAsync(Guid notificationId, bool trackChanges)
        {
            var notification = await FindByCondition(n => n.Id == notificationId, trackChanges)
                .FirstOrDefaultAsync();

            return notification;
        }

        public void CreateNotification(Notification notification)
        {
            Create(notification);
        }

        public void UpdateNotification(Notification notification)
        {
            Update(notification);
        }
    }
}
