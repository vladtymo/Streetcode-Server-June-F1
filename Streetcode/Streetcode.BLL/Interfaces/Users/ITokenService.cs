using System.Security.Claims;
using Streetcode.DAL.Entities.Users;

namespace SoftServerCinema.Security.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user, List<Claim> claims);
        Task<List<Claim>> GetUserClaimsAsync(User user);
        ClaimsPrincipal GetPrincipalFromAccessToken(string? token);
    }
}
