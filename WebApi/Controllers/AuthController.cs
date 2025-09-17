using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;


    [HttpPost("/signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpModel form)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var result = await _authService.SignUp(form);
            if (result.Success == true)
            {
                return Ok(new { success = true, message = "Account created successfully" });
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
