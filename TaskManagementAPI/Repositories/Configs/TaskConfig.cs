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
                .IsClustered(false);

            builder.Property(t => t.TaskSequence)
                .UseIdentityColumn();

            builder.Property(t => t.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasQueryFilter(e => e.DeletedAt == null);

            builder.HasIndex(t => t.ProjectId);
            builder.HasIndex(t => t.Status);
            builder.HasIndex(t => t.Priority);
            builder.HasIndex(t => t.DueDate);
            builder.HasIndex(t => t.AssignedToId);

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

            builder.HasMany(t => t.Labels)
                .WithMany(l => l.Tasks)
                .UsingEntity<Dictionary<string, object>>(j =>
                {
                    j.ToTable("TaskLabels");

                    j.HasOne<Entities.Models.Task>()
                        .WithMany()
                        .HasForeignKey("TaskSequence")
                        .OnDelete(DeleteBehavior.NoAction);

                    j.HasOne<Entities.Models.Label>()
                        .WithMany()
                        .HasForeignKey("LabelSequence")
                        .OnDelete(DeleteBehavior.Cascade);
                });

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
