using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Streetcode.BLL.DTO.Users;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.DAL.Entities.Feedback;
using Streetcode.DAL.Entities.Users;
using System.Security.Cryptography;

namespace Streetcode.BLL.Services.Tokens
{
    public class RefreshTokenService
    {
        private readonly UserManager<User> _userManager;
        private readonly AccessTokenConfiguration _accessTokenConfiguration;
        private readonly ILoggerService _logger;
        public readonly IResponseCookies _responseCookies;
        public RefreshTokenService(UserManager<User> userManager, AccessTokenConfiguration accessTokenConfiguration, ILoggerService logger)
        {
            _userManager = userManager;
            _accessTokenConfiguration = accessTokenConfiguration;
            _logger = logger;
        }

        public async Task<RefreshTokenDTO> GenerateRefreshToken()
        {
            var refreshToken = new RefreshTokenDTO
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7)
            };

            return refreshToken;
        }

        public void SetRefreshToken(RefreshTokenDTO newRefreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            _responseCookies.Append("refreshToken", newRefreshToken.Token);

            user.RefreshToken = newRefreshToken.Token;
            user.Created = newRefreshToken.Created;
            user.Expires = newRefreshToken.Expires;
        }
    }
}
