using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.AdditionalContent;

namespace Streetcode.DAL.Persistence.Configurations.AdditionalContent
{
    internal class StreetcodeTagIndexConfiguration : IEntityTypeConfiguration<StreetcodeTagIndex>
    {
        public void Configure(EntityTypeBuilder<StreetcodeTagIndex> builder)
        { 
            builder.ToTable("streetcode_tag_index", "add_content");

            builder.HasKey(nameof(StreetcodeTagIndex.StreetcodeId), nameof(StreetcodeTagIndex.TagId));

            builder.Property(s => s.IsVisible).IsRequired();

            builder.Property(s => s.Index).IsRequired();

            // builder.Property(s => s.Index).IsRequired().HasAnnotation("Range", new RangeAttribute(0, int.MaxValue));
        }
    }
}
