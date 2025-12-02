using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class Label
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long LabelSequence { get; set; }
        public long? ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string Color { get; set; } = "#3B82F6";
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Project? Project { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}
