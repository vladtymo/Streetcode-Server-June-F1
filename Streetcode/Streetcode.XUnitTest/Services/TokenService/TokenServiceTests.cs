using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Streetcode.BLL.Services.Tokens;
using Streetcode.DAL.Entities.Users;
using Xunit;

namespace Streetcode.XUnitTest.Services.TokenService;

public class TokenServiceTests
{
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly AccessTokenConfiguration _accessTokenConfiguration;
    private readonly BLL.Services.Tokens.TokenService _tokenService;

    public TokenServiceTests()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
        _accessTokenConfiguration = new AccessTokenConfiguration
        {
            SecretKey = "supersecretkeythatshouldbeatleast32characters-long",
            AccessTokenExpirationMinutes = 30,
            Issuer = "Streetcode",
            Audience = "StreetcodeClient"
        };
        _tokenService = new BLL.Services.Tokens.TokenService(_userManagerMock.Object, _accessTokenConfiguration);
    }

    [Fact]
    public async Task GenerateAccessToken_ShouldReturnToken_WhenUserValid()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "testUser", Email = "testuser@example.com" };
        _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

        var claims = _tokenService.GetUserClaimsAsync(user);

        // Act
        var token = await _tokenService.GenerateAccessToken(user, await claims);

        // Assert
        Assert.Multiple(
            () => Assert.NotNull(token),
            () => Assert.IsType<string>(token));
    }  
    
    [Fact]
    public void GetPrincipalFromAccessToken_ShouldReturnClaimsPrincipal_WhenUserValid()
    {
        // Arrange
        var user = new User { Id = Guid.NewGuid(), UserName = "testUser", Email = "testuser@example.com" };
        _userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.CurrentCulture)),
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
        Assert.Multiple(
            () => Assert.NotNull(principal),
            () => Assert.IsType<ClaimsPrincipal>(principal),
            () => Assert.Equal(user.Email, principal.FindFirst(ClaimTypes.Email)?.Value));
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
