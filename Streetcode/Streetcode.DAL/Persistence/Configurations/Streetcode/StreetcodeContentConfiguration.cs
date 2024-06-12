using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.AdditionalContent.Coordinates.Types;
using Streetcode.DAL.Entities.Streetcode;
using Streetcode.DAL.Entities.Toponyms;

namespace Streetcode.DAL.Persistence.Configurations.Streetcode
{
    internal class StreetcodeContentConfiguration : IEntityTypeConfiguration<StreetcodeContent>
    {
        public void Configure(EntityTypeBuilder<StreetcodeContent> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id).ValueGeneratedOnAdd();

            // TODO

            builder.HasMany(s => s.Coordinates)
                .WithOne(c => c.Streetcode)
                .HasForeignKey(c => c.StreetcodeId);

            builder.HasMany(d => d.Toponyms)
                .WithMany(t => t.Streetcodes)
                .UsingEntity<StreetcodeToponym>(
                    st => st.HasOne(s => s.Toponym).WithMany().HasForeignKey(x => x.ToponymId),
                    st => st.HasOne(s => s.Streetcode).WithMany().HasForeignKey(x => x.StreetcodeId))
                .ToTable("streetcode_toponym", "streetcode");
        }
    }
}
