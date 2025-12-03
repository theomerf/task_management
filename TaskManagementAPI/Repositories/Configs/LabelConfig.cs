using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class LabelConfig : IEntityTypeConfiguration<Entities.Models.Label>
    {
        public void Configure(EntityTypeBuilder<Label> builder)
        {
            builder.HasKey(l => l.LabelSequence);

            builder.HasIndex(l => l.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(l => l.LabelSequence)
                .UseIdentityColumn();

            builder.Property(l => l.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(l => l.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Description)
                .HasMaxLength(500);

            builder.Property(l => l.Color)
                .IsRequired()
                .HasMaxLength(7);

            builder.HasOne(l => l.Project)
                .WithMany(p => p.Labels)
                .HasForeignKey(l => l.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(l => l.Tasks)
                .WithOne(t => t.Label)
                .HasForeignKey(t => t.LabelId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
