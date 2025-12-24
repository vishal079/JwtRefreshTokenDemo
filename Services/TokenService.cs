using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenService
{
    private const string SecretKey = "THIS_IS_SUPER_SECRET_KEY_123456789";

    // refreshToken -> expiry
    private static readonly Dictionary<string, DateTime> RefreshTokens = new();

    public string GenerateAccessToken(string username)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(SecretKey));

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(1),
            signingCredentials:
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var refreshToken = Guid.NewGuid().ToString();
        RefreshTokens[refreshToken] = DateTime.UtcNow.AddMinutes(5);
        return refreshToken;
    }

    public bool ValidateRefreshToken(string token)
    {
        if (!RefreshTokens.TryGetValue(token, out var expiry))
            return false;

        if (expiry < DateTime.UtcNow)
        {
            RefreshTokens.Remove(token);
            return false;
        }

        return true;
    }

    // 🔁 Rotation: revoke old, issue new
    public string RotateRefreshToken(string oldToken)
    {
        RefreshTokens.Remove(oldToken);
        return GenerateRefreshToken();
    }

    // 🔒 Logout / Revoke
    public void RevokeRefreshToken(string token)
    {
        RefreshTokens.Remove(token);
    }
}
