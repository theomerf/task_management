using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class ActivityLogConfig : IEntityTypeConfiguration<ActivityLog>
    {
        public void Configure(EntityTypeBuilder<ActivityLog> builder)
        {
            builder.HasKey(al => al.ActivityLogSequence);

            builder.HasIndex(al => al.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(al => al.ActivityLogSequence)
                .UseIdentityColumn();

            builder.Property(al => al.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasIndex(al => new { al.PerformedById, al.CreatedAt });
            builder.HasIndex(al => new { al.RelatedTaskId, al.CreatedAt });
            builder.HasIndex(al => new { al.RelatedProjectId, al.CreatedAt });
            builder.HasIndex(al => al.Type);

            builder.Property(builder => builder.Description)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(al => al.OldValue)
                .HasMaxLength(2000);

            builder.Property(al => al.NewValue)
                .HasMaxLength(2000);

            builder.HasIndex(al => al.CreatedAt);
            builder.HasIndex(al => al.Type);

            builder.HasOne(al => al.PerformedBy)
                .WithMany(a => a.ActivityLogs)
                .HasForeignKey(al => al.PerformedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(al => al.RelatedTask)
                .WithMany(t => t.ActivityLogs)
                .HasForeignKey(al => al.RelatedTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(al => al.RelatedProject)
                .WithMany(p => p.ActivityLogs)
                .HasForeignKey(al => al.RelatedProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
