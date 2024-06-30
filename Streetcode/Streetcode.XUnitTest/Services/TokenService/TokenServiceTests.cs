using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Serilog;
using Streetcode.BLL.Interfaces.Logging;
using Streetcode.BLL.Resources;
using Streetcode.BLL.Services.Logging;
using Streetcode.BLL.Services.Tokens;
using Streetcode.DAL.Entities.Users;
using Xunit;

namespace Streetcode.XUnitTest.Services.TokenService;

public class TokenServiceTests
{
      private readonly Mock<UserManager<User>> _userManagerMock;
      private readonly AccessTokenConfiguration _accessTokenConfiguration;
      private readonly BLL.Services.Tokens.TokenService _tokenService;
      private readonly HttpRequest _request;
      private readonly ILoggerService _logger;

      public TokenServiceTests()
      {
        
      }

      [Fact]
      public async Task GenerateAccessToken_ShouldReturnToken_WhenUserValid()
      {
        // Arrange

        // Act

        // Assert
      }
 
      [Fact]
      public void GetPrincipalFromAccessToken_ShouldReturnClaimsPrincipal_WhenTokenValid()
      {
          // Arrange
          var user = new User { Id = Guid.NewGuid(), UserName = "testUser", Email = "testuser@example.com" };
          var claims = new List<Claim>
          {
              new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
              new Claim(ClaimTypes.NameIdentifier, user.Email),
              new Claim(ClaimTypes.Name, user.UserName),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Role, "Admin")
          };
          var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessTokenConfiguration.SecretKey!));
          var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
          var tokenGenerator = new JwtSecurityToken(
              issuer: _accessTokenConfiguration.Issuer,
              audience: _accessTokenConfiguration.Audience,
              claims: claims,
              expires: DateTime.UtcNow.AddMinutes(_accessTokenConfiguration.AccessTokenExpirationMinutes),
              signingCredentials: signingCredentials);
          var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
          var token = jwtSecurityTokenHandler.WriteToken(tokenGenerator);

          // Act
          var principal = _tokenService.GetPrincipalFromAccessToken(token);

          // Assert
          Assert.NotNull(principal);
          Assert.IsType<ClaimsPrincipal>(principal);
          Assert.Equal(user.Email, principal.FindFirst(ClaimTypes.Email)?.Value);
      }


      [Fact]
      public async Task GetUserClaimsAsync_ShouldReturnCorrectClaims_WhenUserValid()
      {
          // Arrange
          var user = new User { Id = Guid.NewGuid(), UserName = "testUser", Email = "testuser@example.com" };
          _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

          // Act
          var claims = await _tokenService.GetUserClaimsAsync(user);

          // Assert
          var expectedClaims = new List<Claim>
          {
              new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
              new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.CurrentCulture)),
              new Claim(ClaimTypes.NameIdentifier, user.Email),
              new Claim(ClaimTypes.Name, user.UserName),
              new Claim(ClaimTypes.Email, user.Email),
              new Claim(ClaimTypes.Role, "Admin")
          };

          Assert.NotNull(claims);
          Assert.Equal(expectedClaims.Count, claims.Count);
      }
}
