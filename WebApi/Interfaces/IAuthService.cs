using WebApi.Data.Entities;
using WebApi.Models;

namespace WebApi.Interfaces;

public interface IAuthService
{
    public Task<ServiceResult<bool>> SignUp(SignUpModel form);

    public Task<ServiceResult<UserEntity>> CheckCredentials(SignInModel form);

    public Task<ServiceResult<bool>> SendEmailConfirmationAsync(string email);

    public Task<ServiceResult<bool>> ConfirmEmail(string email, string token);
}
