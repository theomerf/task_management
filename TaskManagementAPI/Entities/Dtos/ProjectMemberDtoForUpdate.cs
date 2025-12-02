namespace Entities.Dtos
{
    public record ProjectMemberDtoForUpdate : ProjectMemberDtoForCreation
    {
        public DateTime? LeftAt { get; set; }
    }
}
