using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class TaskConfig : IEntityTypeConfiguration<Entities.Models.Task>
    {
        public void Configure(EntityTypeBuilder<Entities.Models.Task> builder)
        {
            builder.HasKey(t => t.TaskSequence);

            builder.HasIndex(t => t.Id)
                .IsUnique()
                .IsClustered(false)
                .HasFilter("[DeletedAt] IS NULL");

            builder.Property(t => t.TaskSequence)
                .UseIdentityColumn();

            builder.Property(t => t.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasQueryFilter(e => e.DeletedAt == null);

            builder.HasIndex(t => t.ProjectId)
                .HasFilter("[DeletedAt] IS NULL");
            builder.HasIndex(t => t.Status)
                .HasFilter("[DeletedAt] IS NULL");
            builder.HasIndex(t => t.Priority)
                .HasFilter("[DeletedAt] IS NULL");
            builder.HasIndex(t => t.DueDate)
                .HasFilter("[DeletedAt] IS NULL");
            builder.HasIndex(t => t.AssignedToId)
                .HasFilter("[DeletedAt] IS NULL");

            builder.HasIndex(t => new { t.ProjectId, t.Status })
                .HasFilter("[DeletedAt] IS NULL");
            builder.HasIndex(t => new { t.ProjectId, t.DueDate })
                .HasFilter("[DeletedAt] IS NULL");
            builder.HasIndex(t => new { t.AssignedToId, t.Status })
                .HasFilter("[DeletedAt] IS NULL");

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Description)
                .HasMaxLength(2000);

            builder.Property(t => t.EstimatedHours)
                .HasPrecision(8, 2);

            builder.Property(t => t.TotalHoursSpent)
                .HasPrecision(8, 2);

            builder.HasOne(t => t.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.CreatedBy)
                .WithMany(a => a.CreatedTasks)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(t => t.AssignedTo)
                .WithMany(b => b.AssignedTasks)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(t => t.Comments)
                .WithOne(c => c.Task)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Label)
                .WithMany(l => l.Tasks)
                .HasForeignKey(t => t.LabelId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(t => t.Attachments)
                .WithOne(ta => ta.Task)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(t => t.TimeLogs)
                .WithOne(tl => tl.Task)
                .HasForeignKey(tl => tl.TaskId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(t => t.ActivityLogs)
                .WithOne(al => al.RelatedTask)
                .HasForeignKey(al => al.RelatedTaskId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
