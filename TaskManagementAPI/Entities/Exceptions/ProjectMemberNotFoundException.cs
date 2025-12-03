namespace Entities.Exceptions
{
    public class ProjectMemberNotFoundException : NotFoundException
    {
        public ProjectMemberNotFoundException(Guid id) : base($"{id} id'sine sahip proje üyesi bulunamadı.")
        {
        }
    }
}
