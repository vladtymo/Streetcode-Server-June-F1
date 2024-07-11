using Microsoft.AspNetCore.Identity;
using Streetcode.DAL.Entities.Likes;

namespace Streetcode.DAL.Entities.Users
{
    public class User : IdentityUser<Guid>
    {
        public DateOnly BirthDate { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; } = new ();
        public List<Like>? Likes { get; set; }
    }
}