using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class ProjectMemberConfig : IEntityTypeConfiguration<ProjectMember>
    {
        public void Configure(EntityTypeBuilder<ProjectMember> builder)
        {
            builder.HasKey(pm => pm.ProjectMemberSequence);

            builder.HasIndex(pm => pm.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(pm => pm.ProjectMemberSequence)
                .UseIdentityColumn();

            builder.Property(pm => pm.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasIndex(pm => new { pm.ProjectId, pm.AccountId })
                .IsUnique();

            builder.HasIndex(pm => new { pm.AccountId, pm.Role });

            builder.HasQueryFilter(pm => pm.Account!.DeletedAt == null);

            builder.HasOne(pm => pm.Project)
                .WithMany(p => p.Members)
                .HasForeignKey(pm => pm.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pm => pm.Account)
                .WithMany(a => a.ProjectMemberships)
                .HasForeignKey(pm => pm.AccountId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
