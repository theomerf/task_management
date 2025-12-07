namespace Entities.Exceptions
{
    public class AttachmentNotFoundException : NotFoundException
    {
        public AttachmentNotFoundException(Guid id) : base($"{id} is'sine sahip ek bulunamadı.")
        {
        }
    }
}
