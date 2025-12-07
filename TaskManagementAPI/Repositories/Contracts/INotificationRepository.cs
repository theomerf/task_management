using Entities.Dtos;
using Entities.Models;

namespace Repositories.Contracts
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId, bool trackChanges);
        Task<Notification?> GetNotificationByIdAsync(Guid notificiationId, bool trackChanges);
        void CreateNotification(Notification notification);
        void UpdateNotification(Notification notification);
    }
}
