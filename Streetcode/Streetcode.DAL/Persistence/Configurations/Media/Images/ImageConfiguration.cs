using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Media.Images;

namespace Streetcode.DAL.Persistence.Configurations.Media.Images
{
    internal class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable("images", "media");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Id).ValueGeneratedOnAdd();

            builder.Ignore(i => i.Base64);

            builder.Property(i => i.BlobName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(i => i.MimeType)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
