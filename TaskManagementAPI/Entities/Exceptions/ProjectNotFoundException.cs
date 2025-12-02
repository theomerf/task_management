namespace Entities.Exceptions
{
    public class ProjectNotFoundException : NotFoundException
    {
        public ProjectNotFoundException(Guid id) : base($"{id}'sine sahip proje bulunamadı.")
        {
        }
    }
}
