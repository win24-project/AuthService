using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Text;
using WebApi.Data.Context;
using WebApi.Data.Entities;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services;

public class AuthService(UserManager<UserEntity> userManager) : IAuthService
{
    private readonly UserManager<UserEntity> _userManager = userManager;
    public async Task<ServiceResult<bool>> SignUp(SignUpModel form)
    {
        try
        {
            var existingUser = await _userManager.FindByEmailAsync(form.Email);
            if (existingUser != null)
                return ServiceResult<bool>.Conflict("Email already exists");

            var user = new UserEntity { Email = form.Email, UserName = form.Email };
            var userResult = await _userManager.CreateAsync(user, form.Password);
            if (!userResult.Succeeded)
            {
                var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
                return ServiceResult<bool>.BadRequest(errors);
            }

            await _userManager.AddToRoleAsync(user, "Member");

            return ServiceResult<bool>.Ok("Successfully signed up, please confirm your email.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Something went wrong");
        }
    }

    public async Task<ServiceResult<UserEntity>> CheckCredentials(SignInModel form)
    {
        var user = await _userManager.FindByEmailAsync(form.Email);
        if (user is null) return ServiceResult<UserEntity>.Unauthorized("Invalid password or email");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, form.Password);
        if (!isPasswordValid) return ServiceResult<UserEntity>.Unauthorized("Invalid password or email");

        return ServiceResult<UserEntity>.Ok(user);
    }
}
