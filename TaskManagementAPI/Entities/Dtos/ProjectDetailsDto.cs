using Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record ProjectDetailsDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Description { get; init; }
        public string Icon { get; init; } = null!;
        public string Color { get; init; } = null!;
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public ProjectStatus Status { get; init; }
        public ProjectVisibility Visibility { get; init; }
        public int TaskCount { get; init; }
        public int CompletedTaskCount { get; init; }
        public ICollection<TaskDto> Tasks { get; init; } = new List<TaskDto>();
        public ICollection<LabelDto> Labels { get; init; } = new List<LabelDto>();
        public ProjectSettingDto Settings { get; init; } = null!;
    }
}
