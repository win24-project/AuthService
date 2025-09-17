namespace WebApi.Interfaces;

public interface IAccessTokenService
{
    public string GenerateAccessTokenAsync(string userId);
}
