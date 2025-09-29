using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Data.Entities;
using WebApi.Interfaces;

namespace WebApi.Services;

public class AccessTokenService(IConfiguration configuration, UserManager<UserEntity> userManager) : IAccessTokenService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly UserManager<UserEntity> _userManager = userManager;
    public async Task<string> GenerateAccessTokenAsync(UserEntity user)
    {
        string issuer = _configuration["JwtIssuer"];
        string audience = _configuration["JwtAudience"];
        string key = _configuration["JwtPublicKey"];

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email)
            };
        if(roles.Count > 0)
        {
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
        } else
        {
            claims.Add(new Claim(ClaimTypes.Role, "Member"));
        }
        

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
