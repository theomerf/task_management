namespace Entities.Exceptions
{
    public class LabelNotFoundException : NotFoundException
    {
        public LabelNotFoundException(Guid id) : base($"{id} id'sine sahip etiket bulunamadı.")
        {
        }
    }
}
