using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface INotificationService
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId);
        System.Threading.Tasks.Task MarkNotificationAsReadAsync(string userId, Guid notificationId);
        System.Threading.Tasks.Task ArchiveNotificationAsync(string userId, Guid notificationId);
    }
}
