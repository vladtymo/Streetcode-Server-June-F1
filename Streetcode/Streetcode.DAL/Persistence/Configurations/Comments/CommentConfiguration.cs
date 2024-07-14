using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Comments;

namespace Streetcode.DAL.Configurations.Comments
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();

            builder.Property(c => c.CommentContent)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            builder.HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(c => c.Streetcode)
                .WithMany(s => s.Comments)
                .HasForeignKey(c => c.StreetcodeId)
                .OnDelete(DeleteBehavior.Cascade);



            builder.ToTable("Comments");
        }
    }
}