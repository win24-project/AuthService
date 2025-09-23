using WebApi.Data.Entities;

namespace WebApi.Interfaces;

public interface IAccessTokenService
{
    public Task<string> GenerateAccessTokenAsync(UserEntity user);
}
