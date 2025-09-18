using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Interfaces;

namespace WebApi.Services;

public class AccessTokenService(SecretClient secretClient) : IAccessTokenService
{
    public async Task<string> GenerateAccessTokenAsync(string userId)
    {
        KeyVaultSecret issuer = await secretClient.GetSecretAsync("JwtIssuer");
        KeyVaultSecret audience = await secretClient.GetSecretAsync("JwtAudience");
        KeyVaultSecret key = await secretClient.GetSecretAsync("JwtSecretKey");

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
            issuer: issuer.Value,
            audience: audience.Value,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.Value)), 
                SecurityAlgorithms.HmacSha256)
        );

        var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwt = JwtSecurityTokenHandler.WriteToken(token);

        return jwt;
    }
}
