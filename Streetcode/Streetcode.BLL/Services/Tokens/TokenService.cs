using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SoftServerCinema.Security.Interfaces;
using Streetcode.DAL.Entities.Users;

namespace Streetcode.BLL.Services.Users;

public class TokenService : ITokenService
{
    private readonly UserManager<User> _userManager;
    private readonly AccessTokenConfiguration _accessTokenConfiguration;
    
    public TokenService(UserManager<User> userManager, AccessTokenConfiguration accessTokenConfiguration)
    {
        _userManager = userManager;
        _accessTokenConfiguration = accessTokenConfiguration;
    }
    
    public async Task<string> GenerateAccessToken(User user, List<Claim> claims)
    {
        DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_accessTokenConfiguration.AccessTokenExpirationMinutes));

        SymmetricSecurityKey securityKey = new (Encoding.UTF8.GetBytes(_accessTokenConfiguration.SecretKey!));

        SigningCredentials signingCredentials = new (securityKey, SecurityAlgorithms.HmacSha256);
        JwtSecurityToken tokenGenerator = new (
            issuer: _accessTokenConfiguration.Issuer,
            audience: _accessTokenConfiguration.Audience,
            claims: claims,
            expires: expiration,
            signingCredentials: signingCredentials);

        JwtSecurityTokenHandler jwtSecurityTokenHandler = new ();
        var token = jwtSecurityTokenHandler.WriteToken(tokenGenerator);
        return token;
    }

    public async Task<List<Claim>> GetUserClaimsAsync(User user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        List<Claim> claims = new ()
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), 
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.CurrentCulture)),
            new Claim(ClaimTypes.NameIdentifier, user.Email), 
            new Claim(ClaimTypes.Name, user.UserName), 
            new Claim(ClaimTypes.Email, user.Email), 
            new Claim(ClaimTypes.Role, roles.FirstOrDefault() !)
        };

        return claims;
    }

    public ClaimsPrincipal GetPrincipalFromAccessToken(string? token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = _accessTokenConfiguration.Audience,
            ValidateIssuer = true,
            ValidIssuer = _accessTokenConfiguration.Issuer,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessTokenConfiguration.SecretKey!)),
            ValidateLifetime = true
        };
        
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        return principal;
    }
}