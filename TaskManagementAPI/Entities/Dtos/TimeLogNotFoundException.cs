using Entities.Exceptions
namespace Entities.Dtos
{
    public class TimeLogNotFoundException : NotFoundException
    {
        public TimeLogNotFoundException(Guid id) : base($"{id} id'sine sahip görev zaman raporu bulunamadı.")
        {
        }
    }
}
