namespace Entities.Dtos
{
    public record TimeLogDto
    {
        public Guid Id { get; init; }
        public string LoggedByEmail { get; init; } = null!;
        public string TimeLogCategoryName { get; init; } = null!;
        public string TimeLogCategoryColor { get; init; } = null!;
        public decimal Hours { get; init; }
    }

    public record TimeLogCategoryDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Color { get; init; } = "#3B82F6";
    }
}
