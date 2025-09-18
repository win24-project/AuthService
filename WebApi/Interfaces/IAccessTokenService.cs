namespace WebApi.Interfaces;

public interface IAccessTokenService
{
    public Task<string> GenerateAccessTokenAsync(string userId);
}
