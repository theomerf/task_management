namespace Entities.Exceptions
{
    public class NotificationNotFoundException : NotFoundException
    {
        public NotificationNotFoundException(Guid id) : base($"{id} id'sine sahip bildirim bulunamadı.")
        {
        }
    }
}
