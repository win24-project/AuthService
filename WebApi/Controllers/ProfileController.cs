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

    [HttpPost("profile/change-membership")]
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

    [HttpPost("profile/change-subscription-status")]
    public async Task<IActionResult> ChangeSubscriptionStatus([FromQuery] string status)
    {
        try
        {
            if (String.IsNullOrEmpty(status)) return BadRequest("No status was provided");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _profileService.ChangeSubscriptionStatus(userId, status);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok("Subscription status was updated in profile successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("profile/change-customer-id")]
    public async Task<IActionResult> ChangeCustomerId([FromQuery] string customerID)
    {
        try
        {
            if (String.IsNullOrEmpty(customerID)) return BadRequest("No customer id was provided");

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();

            var result = await _profileService.ChangeCustomerId(userId, customerID);
            if (!result.Success) return StatusCode(result.StatusCode, result.ErrorMessage);

            return Ok("Customer id was updated in profile successfully");
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("profile/add-subscription")]
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
