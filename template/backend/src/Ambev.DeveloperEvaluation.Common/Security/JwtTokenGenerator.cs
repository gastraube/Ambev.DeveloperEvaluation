using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ambev.DeveloperEvaluation.Common.Security;

public sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _config;
    public JwtTokenGenerator(IConfiguration config) => _config = config;

    public string GenerateToken(Guid userId, string username, IEnumerable<string>? roles = null)
    {
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var key = _config["Jwt:Key"];
        var expires = int.TryParse(_config["Jwt:ExpiresMinutes"], out var m) ? m : 60;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, username),
            new(ClaimTypes.Name, username),
        };

        if (roles != null)
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expires),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}