using Entities.Dtos;
using Entities.Exceptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class NotificationManager : INotificationService
    {
        private readonly IRepositoryManager _manager;

        public NotificationManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            var notifications = _manager.Notification.GetUserNotificationsAsync(userId, false);

            return notifications;
        }

        private async Task<Notification> GetNotificationByIdForServiceAsync(Guid notificationId, bool trackChanges)
        {
            var notification = await _manager.Notification.GetNotificationByIdAsync(notificationId, trackChanges);

            if (notification == null)
                throw new NotificationNotFoundException(notificationId);

            return notification;
        }

        public async System.Threading.Tasks.Task MarkNotificationAsReadAsync(string userId, Guid notificationId)
        {
            var notification = await GetNotificationByIdForServiceAsync(notificationId, true);
            
            if (notification.RecipientId != userId)
                throw new ForbiddenException("Bu bildirim üzerinde işlem yapma yetkiniz yok.");

            notification.ReadAt = DateTime.UtcNow;

            await _manager.SaveAsync();
        }

        public async System.Threading.Tasks.Task ArchiveNotificationAsync(string userId, Guid notificationId)
        {
            var notification = await GetNotificationByIdForServiceAsync(notificationId, true);

            if (notification.RecipientId != userId)
                throw new ForbiddenException("Bu bildirim üzerinde işlem yapma yetkiniz yok.");

            notification.IsArchived = true;

            await _manager.SaveAsync();
        }
    }
}
