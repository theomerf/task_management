namespace Entities.Exceptions
{
    public class TaskAttachmentNotFoundException : NotFoundException
    {
        public TaskAttachmentNotFoundException(Guid id) : base($"{id} is'sine sahip görev eki bulunamadı.")
        {
        }
    }
}
