using System.Security.Claims;
using Streetcode.BLL.DTO.Users;
using Streetcode.DAL.Entities.Users;

namespace SoftServerCinema.Security.Interfaces
{
    public interface ITokenService
    {
        Task<string> GenerateAccessToken(User user, List<Claim> claims);
        Task<List<Claim>> GetUserClaimsAsync(User user);
        ClaimsPrincipal GetPrincipalFromAccessToken(string? token);
        Task<RefreshTokenDTO> GenerateRefreshToken();
        Task SetRefreshToken(RefreshTokenDTO newRefreshToken, User user);
        Task<string> GenerateTokens(User user);
    }
}
