using WebApi.Models;

namespace WebApi.Interfaces;

public interface IAuthService
{
    public Task<ServiceResult<bool>> SignUp(SignUpModel form);
}
