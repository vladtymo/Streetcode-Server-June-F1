using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Streetcode.DAL.Entities.Users;
using Streetcode.DAL.Persistence.Converters;

namespace Streetcode.DAL.Persistence.Configurations.Identity.Users
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users", "Users");
                        
            builder.Property(u => u.BirthDate)
                .HasConversion<DateOnlyConverter>()
                .HasColumnType("date");

            builder.Property(u => u.FirstName)
                .HasMaxLength(50);

            builder.Property(u => u.LastName)
                .HasMaxLength(50);
        }
    }
}
