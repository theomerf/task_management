using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public partial class TimeLogConfig : IEntityTypeConfiguration<TimeLog>
    {
        public void Configure(EntityTypeBuilder<TimeLog> builder)
        {
            builder.HasKey(tl => tl.TimeLogSequence);

            builder.HasIndex(tl => tl.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(tl => tl.TimeLogSequence)
                .UseIdentityColumn();

            builder.Property(tl => tl.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasQueryFilter(tl => !tl.IsDeleted);

            builder.HasQueryFilter(tl => tl.DeletedAt == null);

            builder.HasIndex(tl => tl.Date);

            builder.Property(tl => tl.Notes)
                .HasMaxLength(2000);

            builder.Property(tl => tl.Hours)
                .HasPrecision(8, 2);

            builder.HasOne(tl => tl.Task)
                .WithMany(t => t.TimeLogs)
                .HasForeignKey(tl => tl.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(tl => tl.LoggedBy)
                .WithMany(a => a.TimeLogs)
                .HasForeignKey(tl => tl.LoggedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(tl => tl.TimeLogCategory)
                .WithMany(tlc => tlc.TimeLogs)
                .HasForeignKey(tl => tl.TimeLogCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
