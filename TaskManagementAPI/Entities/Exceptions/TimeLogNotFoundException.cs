namespace Entities.Exceptions
{
    public class TimeLogNotFoundException : NotFoundException
    {
        public TimeLogNotFoundException(Guid id) : base($"{id} id'sine sahip görev zaman raporu bulunamadı.")
        {
        }
    }
}
