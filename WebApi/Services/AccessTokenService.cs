using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Interfaces;

namespace WebApi.Services;

public class AccessTokenService(IConfiguration config) : IAccessTokenService
{
    private readonly IConfiguration _config = config;
    public string GenerateAccessTokenAsync(string userId)
    {
        var issuer = _config["Jwt:Issuer"];
        var audience = _config["Jwt:Audience"];
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, userId)
            };
        var header = new JwtHeader
            {
                { "alg", "HS256" },
                { "typ", "JWT" }
            };
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwt = JwtSecurityTokenHandler.WriteToken(token);

        return jwt;
    }
}
