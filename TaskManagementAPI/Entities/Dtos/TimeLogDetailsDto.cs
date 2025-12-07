using Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record TimeLogDetailsDto
    {
        public Guid Id { get; init; }
        public string LoggedByEmail { get; init; } = null!;
        public string LoggedByFirstName { get; init; } = null!;
        public string LoggedByLastName { get; init; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Hours { get; init; }
        public DateTime Date { get; init; }
        public string? Notes { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
        public TimeLogCategoryDto TimeLogCategory { get; init; } = null!;
    }

    public record TimeLogCategoryDetailsDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Color { get; init; } = "#3B82F6";
        public ICollection<TimeLogDto> TimeLogs { get; init; } = new List<TimeLogDto>();
    }
}
