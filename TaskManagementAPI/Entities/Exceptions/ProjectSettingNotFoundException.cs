namespace Entities.Exceptions
{
    public class ProjectSettingNotFoundException : NotFoundException
    {
        public ProjectSettingNotFoundException(Guid id) : base($"{id} id'sine sahip projenin ayarları bulunamadı.")
        {
        }
    }
}
