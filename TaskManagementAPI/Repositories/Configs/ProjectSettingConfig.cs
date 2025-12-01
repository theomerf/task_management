using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    internal class ProjectSettingConfig : IEntityTypeConfiguration<ProjectSetting>
    {
        public void Configure(EntityTypeBuilder<ProjectSetting> builder)
        {
            builder.HasKey(ps => ps.ProjectSettingSequence);

            builder.HasIndex(ps => ps.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(ps => ps.ProjectSettingSequence)
                .UseIdentityColumn();

            builder.Property(ps => ps.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.Property(ps => ps.CustomFields)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(ps => ps.Project)
                .WithOne(p => p.Settings)
                .HasForeignKey<ProjectSetting>(ps => ps.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
