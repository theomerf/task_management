using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public partial class TimeLogConfig
    {
        public class TimeLogCategoryConfig : IEntityTypeConfiguration<TimeLogCategory>
        {
            public void Configure(EntityTypeBuilder<TimeLogCategory> builder)
            {
                builder.HasKey(tlc => tlc.TimeLogCategorySequence);

                builder.HasIndex(tlc => tlc.Id)
                    .IsUnique()
                    .IsClustered(false);

                builder.Property(tlc => tlc.TimeLogCategorySequence)
                    .UseIdentityColumn();

                builder.Property(tlc => tlc.Id)
                    .HasDefaultValueSql("NEWSEQUENTIALID()");

                builder.Property(tlc => tlc.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                builder.Property(l => l.Color)
                    .IsRequired()
                    .HasMaxLength(7);

                builder.HasMany(tlc => tlc.TimeLogs)
                    .WithOne(tl => tl.TimeLogCategory)
                    .HasForeignKey(tl => tl.TimeLogCategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(tlc => tlc.Project)
                    .WithMany(p => p.TimeLogCategories)
                    .HasForeignKey(tlc => tlc.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade);
            }
        }
    }
}
