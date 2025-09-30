using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using WebApi.Interfaces;
using WebApi.Models;

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

    [HttpGet("/profile/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetCustomerId(string userId)
    {
        try
        {
            var customerId = await _profileService.GetCustomerId(userId);
            if(string.IsNullOrEmpty(customerId))
                return NotFound();

            return Ok(customerId);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("/profile/change-membership-plan")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangeMembership([FromQuery] string customerId, [FromQuery] string membershipPlan)
    {
        if (String.IsNullOrEmpty(customerId)) return BadRequest("No user id was provided");
        if (String.IsNullOrEmpty(membershipPlan)) return BadRequest("No membership plan was provided");

        try
        {
            var result = await _profileService.ChangeMembershipPlan(customerId, membershipPlan);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok("Membership plan was updated successfully");
        } catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("/profile/change-subscription-status")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangeSubscriptionStatus([FromQuery] string customerId, [FromQuery] string status)
    {
        if (String.IsNullOrEmpty(customerId)) return BadRequest("No user id was provided");
        if (String.IsNullOrEmpty(status)) return BadRequest("No status was provided");

        try
        {
            var result = await _profileService.ChangeSubscriptionStatus(customerId, status);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok("Membership plan was updated successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("/profile/add-subscription")]
    [AllowAnonymous]
    public async Task<IActionResult> ChangeCustomerId([FromBody] SubscriptionRequest subscriptionRequest)
    {
        try
        {
            if (String.IsNullOrEmpty(subscriptionRequest.CustomerId)) return BadRequest("No customer id was provided");
            if (String.IsNullOrEmpty(subscriptionRequest.SubscriptionStatus)) return BadRequest("No status was provided");

            var result = await _profileService.AddSubscription(subscriptionRequest.AccountId, subscriptionRequest.SubscriptionStatus, subscriptionRequest.CustomerId);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok("Customer id was updated in profile successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }
}
