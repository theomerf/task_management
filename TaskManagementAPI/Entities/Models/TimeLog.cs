using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class TimeLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TimeLogSequence { get; set; }
        public long TaskId { get; set; }
        public string LoggedById { get; set; } = null!;
        public long? TimeLogCategoryId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Hours => (decimal)(EndTime - StartTime).TotalHours;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        [NotMapped]
        public bool IsDeleted => DeletedAt.HasValue;
        public Task? Task { get; set; }
        public Account? LoggedBy { get; set; }
        public TimeLogCategory? TimeLogCategory { get; set; }
    }

    public class TimeLogCategory
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long TimeLogCategorySequence { get; set; }
        public long ProjectId { get; set; }
        public string Name { get; set; } = null!;
        public string Color { get; set; } = "#3B82F6";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public Project? Project { get; set; }
        public ICollection<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    }
}
