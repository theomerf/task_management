using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class AccountConfig : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasQueryFilter(a => a.DeletedAt == null);

            builder.HasIndex(a => a.Email)
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL");

            builder.HasIndex(a => a.DeletedAt)
                .HasFilter("[DeletedAt] IS NOT NULL");

            builder.Property(a => a.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.LastName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.AvatarUrl)
                .HasMaxLength(500);

            builder.Property(a => a.RefreshToken)
                .HasMaxLength(500);

            builder.HasMany(a => a.CreatedProjects)
                .WithOne(p => p.CreatedBy)
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(a => a.CreatedTasks)
                .WithOne(t => t.CreatedBy)
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(a => a.AssignedTasks)
                .WithOne(t => t.AssignedTo)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(a => a.ProjectMemberships)
                .WithOne(pm => pm.Account)
                .HasForeignKey(pm => pm.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Comments)
                .WithOne(c => c.Author)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.TimeLogs)
                .WithOne(tl => tl.LoggedBy)
                .HasForeignKey(tl => tl.LoggedById)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Notifications)
                .WithOne(n => n.Recipient)
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.ActivityLogs)
                .WithOne(al => al.PerformedBy)
                .HasForeignKey(al => al.PerformedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}