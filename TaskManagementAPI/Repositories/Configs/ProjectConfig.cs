using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class ProjectConfig : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.HasKey(p => p.ProjectSequence);

            builder.HasIndex(p => p.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(p => p.ProjectSequence)
                .UseIdentityColumn();

            builder.Property(p => p.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasIndex(e => e.CreatedById);
            builder.HasIndex(e => e.Status);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .HasMaxLength(2000);

            builder.Property(p => p.Icon)
                .HasMaxLength(1);

            builder.Property(p => p.Color)
                .HasMaxLength(7);

            builder.HasOne(p => p.CreatedBy)
                .WithMany(a => a.CreatedProjects)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Members)
                .WithOne(pm => pm.Project)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Labels)
                .WithOne(l => l.Project)
                .HasForeignKey(l => l.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Settings)
                .WithOne(ps => ps.Project)
                .HasForeignKey<ProjectSetting>(ps => ps.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.ActivityLogs)
                .WithOne(al => al.RelatedProject)
                .HasForeignKey(al => al.RelatedProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
