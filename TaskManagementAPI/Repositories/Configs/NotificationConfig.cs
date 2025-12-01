using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class NotificationConfig : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasKey(n => n.NotificationSequence);

            builder.HasIndex(n => n.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(n => n.NotificationSequence)
                .UseIdentityColumn();

            builder.Property(n => n.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasIndex(n => n.RecipientId);
            builder.HasIndex(n => n.IsRead);

            builder.HasQueryFilter(n => n.Recipient != null && n.Recipient.DeletedAt == null);

            builder.Property(n => n.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(n => n.Message)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(n => n.Icon)
                .HasMaxLength(1);

            builder.HasOne(n => n.Recipient)
                .WithMany(a => a.Notifications)
                .HasForeignKey(n => n.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(n => n.Initiator)
                .WithMany()
                .HasForeignKey(n => n.InitiatorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(n => n.RelatedTask)
                .WithMany()
                .HasForeignKey(n => n.RelatedTaskId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(n => n.RelatedProject)
                .WithMany()
                .HasForeignKey(n => n.RelatedProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
