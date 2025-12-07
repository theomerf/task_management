namespace Entities.Exceptions
{
    public class CommentNotFoundException : NotFoundException
    {
        public CommentNotFoundException(Guid id) : base($"{id} id'sine sahip yorum bulunamadı.")
        {
        }
    }
}
