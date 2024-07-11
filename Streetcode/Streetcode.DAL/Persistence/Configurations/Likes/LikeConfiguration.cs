using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Likes;

namespace Streetcode.DAL.Persistence.Configurations.Likes
{
    public class LikeConfiguration : IEntityTypeConfiguration<Like>
    {
        public void Configure(EntityTypeBuilder<Like> builder)
        {
            builder.HasKey(d => new { d.UserId, d.streetcodeId });

            builder.HasOne(d => d.Streetcode)
                .WithMany(d => d.Likes)
                .HasForeignKey(d => d.streetcodeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(d => d.User)
                .WithMany(d => d.Likes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
