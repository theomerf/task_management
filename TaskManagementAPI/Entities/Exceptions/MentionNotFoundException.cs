namespace Entities.Exceptions
{
    public class MentionNotFoundException : NotFoundException
    {
        public MentionNotFoundException(Guid id) : base($"{id} id'sine sahip yorum bahsetmesi bulunamadı.")
        {
        }
    }
}
