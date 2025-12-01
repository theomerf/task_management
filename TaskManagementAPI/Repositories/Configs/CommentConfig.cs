using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.CommentSequence);

            builder.HasIndex(c => c.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(c => c.CommentSequence)
                .UseIdentityColumn();

            builder.Property(c => c.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasQueryFilter(c => c.DeletedAt == null);

            builder.HasIndex(c => c.TaskId);

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(c => c.Reactions)
                .HasColumnType("nvarchar(max)");

            builder.HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.Author)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(c => c.Replies)
                .WithOne(c => c.ParentComment)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(c => c.Attachments)
                .WithOne(ta => ta.Comment)
                .HasForeignKey(ta => ta.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Mentions)
                .WithOne(m => m.Comment)
                .HasForeignKey(m => m.CommentId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
