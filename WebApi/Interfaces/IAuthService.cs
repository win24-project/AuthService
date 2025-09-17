using WebApi.Data.Entities;
using WebApi.Models;

namespace WebApi.Interfaces;

public interface IAuthService
{
    public Task<ServiceResult<bool>> SignUp(SignUpModel form);

    public Task<ServiceResult<UserEntity>> CheckCredentials(SignInModel form);
}
