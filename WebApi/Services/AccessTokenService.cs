using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Interfaces;

namespace WebApi.Services;

public class AccessTokenService(IConfiguration configuration) : IAccessTokenService
{
    private readonly IConfiguration _configuration = configuration;
    public async Task<string> GenerateAccessTokenAsync(string userId)
    {
        string issuer = _configuration["JwtIssuer"];
        string audience = _configuration["JwtAudience"];
        string key = _configuration["JwtSecretKey"];

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
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)), 
                SecurityAlgorithms.HmacSha256)
        );

        var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwt = JwtSecurityTokenHandler.WriteToken(token);

        return jwt;
    }
}
