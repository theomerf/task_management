namespace Entities.Dtos
{
    public record ProjectSettingDtoForUpdate : ProjectSettingDtoForCreation
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
