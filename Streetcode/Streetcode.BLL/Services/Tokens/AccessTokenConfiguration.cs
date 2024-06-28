namespace Streetcode.BLL.Services.Tokens;

public class AccessTokenConfiguration
{
    public double AccessTokenExpirationMinutes { get; set; }
    public string? SecretKey { get; set; } = string.Empty;
    public string? Issuer { get; set; } = string.Empty;
    public string? Audience { get; set; } = string.Empty;
}