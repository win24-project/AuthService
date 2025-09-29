using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService, IAccessTokenService accessTokenService) : Controller
{
    private readonly IAuthService _authService = authService;
    private readonly IAccessTokenService _accessTokenService = accessTokenService;


    [HttpPost("/signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpModel form)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _authService.SignUp(form);
            if (result.Success == true)
            {
                return Ok(new { success = true, message = "Account created successfully, Please confirm your email" });
            }
            return StatusCode(result.StatusCode, result.ErrorMessage);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("/signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInModel form)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var CheckCredentialsResult = await _authService.CheckCredentials(form);
            if (CheckCredentialsResult.Data is null) return StatusCode(CheckCredentialsResult.StatusCode, CheckCredentialsResult.ErrorMessage!);
            var user = CheckCredentialsResult.Data;

            var accesstoken = await _accessTokenService.GenerateAccessTokenAsync(user);
            if (string.IsNullOrEmpty(accesstoken))
                return StatusCode(500, "Failed To create the access token");

            return Ok(new { success = true, message = "You Signed in successfully", token = accesstoken });

        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("/confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string email, [FromQuery] string token)
    {
        try
        {
            var result = await _authService.ConfirmEmail(email, token);
            if (result.Success == true)
            {
                return Ok(new { success = true, message = "Ýour email is no confirmed and" });
            }
            return StatusCode(result.StatusCode, result.ErrorMessage);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("/resend-email-confirmation")]
    public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailModel resendEmailModel)
    {
        try
        {
            var result = await _authService.SendEmailConfirmationAsync(resendEmailModel.Email);
            if (result.Success == true)
            {
                return Ok("Confirmation link is sent to your email. Please check your inbox");
            }
            return StatusCode(result.StatusCode, result.ErrorMessage);

        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }



}
