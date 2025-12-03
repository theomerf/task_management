namespace Entities.Exceptions
{
    public class TimeLogCategoryNotFoundException : NotFoundException
    {
        public TimeLogCategoryNotFoundException(Guid id) : base($"{id} id'sine sahip zaman raporu kategorisi bulunamadı.")
        {
        }
    }
}
