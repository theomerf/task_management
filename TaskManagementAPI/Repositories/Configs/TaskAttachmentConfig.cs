using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class TaskAttachmentConfig : IEntityTypeConfiguration<TaskAttachment>
    {
        public void Configure(EntityTypeBuilder<TaskAttachment> builder)
        {
            builder.HasKey(ta => ta.AttachmentSequence);

            builder.HasIndex(ta => ta.Id)
                .IsUnique()
                .IsClustered(false)
                .HasFilter("[DeletedAt] IS NULL");

            builder.Property(ta => ta.AttachmentSequence)
                .UseIdentityColumn();

            builder.Property(ta => ta.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasQueryFilter(ta => ta.DeletedAt == null);

            builder.HasIndex(ta => ta.TaskId)
                .HasFilter("[DeletedAt] IS NULL");

            builder.Property(ta => ta.FileName)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ta => ta.FileType)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ta => ta.FileUrl)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(ta => ta.ThumbnailUrl)
                .HasMaxLength(255);

            builder.HasOne(ta => ta.Task)
                .WithMany(t => t.Attachments)
                .HasForeignKey(ta => ta.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ta => ta.Comment)
                .WithMany(t => t.Attachments)
                .HasForeignKey(ta => ta.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ta => ta.UploadedBy)
                .WithMany()
                .HasForeignKey(ta => ta.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
