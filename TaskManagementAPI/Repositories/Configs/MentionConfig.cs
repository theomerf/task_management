using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Configs
{
    public class MentionConfig : IEntityTypeConfiguration<Mention>
    {
        public void Configure(EntityTypeBuilder<Mention> builder)
        {
            builder.HasKey(m => m.MentionSequence);

            builder.HasIndex(m => m.Id)
                .IsUnique()
                .IsClustered(false);

            builder.Property(m => m.MentionSequence)
                .UseIdentityColumn();

            builder.Property(m => m.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder.HasQueryFilter(m => m.Comment != null && m.Comment.DeletedAt == null);

            builder.HasOne(m => m.Comment)
                .WithMany(c => c.Mentions)
                .HasForeignKey(m => m.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(m => m.MentionedUser)
                .WithMany()
                .HasForeignKey(m => m.MentionedUserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
