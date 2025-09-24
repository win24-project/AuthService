using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebApi.Interfaces;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProfileController(IProfileService profileService) : ControllerBase
{
    private readonly IProfileService _profileService = profileService;
    [HttpGet("/profile")]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _profileService.GetProfile(userId);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok(result.Data);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("profile/add-membership")]
    public async Task<IActionResult> ChangeMembership([FromQuery] int membershipId)
    {
        try
        {
            if(membershipId == 0) return BadRequest("No membership id was provided");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _profileService.ChangeMembership(userId, membershipId);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok("Membership was updated in profile successfully");
        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }
}
