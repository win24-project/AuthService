using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Text;
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

            var user = new UserEntity { Email = form.Email, UserName = form.Email, CustomerId = "" };
            var userResult = await _userManager.CreateAsync(user, form.Password);
            if (!userResult.Succeeded)
            {
                var errors = string.Join(", ", userResult.Errors.Select(e => e.Description));
                return ServiceResult<bool>.BadRequest(errors);
            }

            await _userManager.AddToRoleAsync(user, "Member");
            await SendEmailConfirmationAsync(user.Email);

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

        bool emailIsConfirmed = await _userManager.IsEmailConfirmedAsync(user);
        if (!emailIsConfirmed) 
            return ServiceResult<UserEntity>.Unauthorized("You must confirm your email");

        return ServiceResult<UserEntity>.Ok(user);
    }

    public async Task<ServiceResult<bool>> SendEmailConfirmationAsync(string email)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return ServiceResult<bool>.NotFound($"Could not find user with email {email}");

            var confirmEmailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var tokenBytes = Encoding.UTF8.GetBytes(confirmEmailToken);
            var encodedToken = Convert.ToBase64String(tokenBytes);

            string subject = "Confirm Email";
            string htmlContent = GetConfirmEmailHtmlContent(email, encodedToken);
            string textContent = $"Please copy this link https://happy-mud-02a876a03.2.azurestaticapps.net/confirm-email?email={email}&token={encodedToken} into your browser to confirm your email";
            List<string> recipients = [email];

            var client = new HttpClient();
            await client.PostAsJsonAsync("https://group-project-emailservice-ebesatdzd9h4b2c2.swedencentral-01.azurewebsites.net/index.html", 
                new
                {
                    subject,
                    textContent,
                    htmlContent,
                    recipients
                });
            return ServiceResult<bool>.Ok($"Email sent successfully to {email}");
        } catch (Exception err)
        {
            Debug.WriteLine(err.Message);
            return ServiceResult<bool>.Error("Failed to send email confirmation link");
        }
    }

    public async Task<ServiceResult<bool>> ConfirmEmail(string email, string token)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return ServiceResult<bool>.BadRequest($"User does not exist with email: {email}");
            var tokenBytes = Convert.FromBase64String(token);
            var decodedToken = Encoding.UTF8.GetString(tokenBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);
            if (!result.Succeeded) return ServiceResult<bool>.BadRequest("Token is invalid");

            return ServiceResult<bool>.Ok("Successfully confirmed email");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return ServiceResult<bool>.Error("Could not confirm email");
        }
    }

    private string GetConfirmEmailHtmlContent(string email, string token)
    {
        return $@"
                <!DOCTYPE html>
                <html>
                <head>
                  <meta charset=""UTF-8"">
                  <title>Email Confirmation</title>
                </head>
                <body style=""margin:0; padding:0; font-family: Arial, sans-serif; background-color:#f4f4f4;"">
                  <table align=""center"" border=""0"" cellpadding=""0"" cellspacing=""0"" width=""100%"" style=""max-width:600px; background-color:#ffffff; margin-top:30px; border-radius:8px; box-shadow:0 0 10px rgba(0,0,0,0.1);"">
                    <tr>
                      <td align=""center"" style=""padding: 40px 30px 20px 30px;"">
                        <h2 style=""color:#FF6B35;"">Confirm Your Email</h2>
                        <p style=""color:#666666;"">Click the button below to confirm your email</p>
                      </td>
                    </tr>
                    <tr>
                      <td align=""center"" style=""padding: 20px;"">
                        <a href=""https://happy-mud-02a876a03.2.azurestaticapps.net/confirm-email?email={email}&token={token}"" 
                           style=""background-color:#FF6B35; color:#eeefff; padding:12px 24px; text-decoration:none; border-radius:5px; display:inline-block; font-weight:bold;"">
                          Confirm Email
                        </a>
                      </td>
                    </tr>
                    <tr>
                      <td align=""center"" style=""padding: 20px 30px 40px 30px; font-size:12px; color:#aaaaaa;"">
                        If the button doesn’t work, copy and paste the following link into your browser:<br>
                        <a href=""https://happy-mud-02a876a03.2.azurestaticapps.net/confirm-email?email={email}&token={token}"" style=""color:#FF6B35;"">
                          https://happy-mud-02a876a03.2.azurestaticapps.net/confirm-email?email={email}&token={token}
                        </a>
                      </td>
                    </tr>
                  </table>
                </body>
                </html>";
    }
}
