namespace Entities.Exceptions
{
    public class TaskNotFoundException : NotFoundException
    {
        public TaskNotFoundException(Guid id) : base($"{id} id'sine sahip görev bulunamadı.")
        {
        }
    }
}
