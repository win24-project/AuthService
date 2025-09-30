using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using WebApi.Data.Entities;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Services;

public class ProfileService(UserManager<UserEntity> userManager) : IProfileService
{
    private readonly UserManager<UserEntity> _userManager = userManager;

    public async Task<ServiceResult<bool>> ChangeMembership(string userId, int membershipId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ServiceResult<bool>.NotFound("Could not find user");

            user.MembershipId = membershipId;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return ServiceResult<bool>.Error($"Failed to update membership");
            }

            return ServiceResult<bool>.Ok(true);

        } catch (Exception ex)
        {
            return ServiceResult<bool>.Error("Failed to add membership in profile");
        }
    }

    public async Task<string> GetCustomerId(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return "";

            string customerId = user.CustomerId;
            if (string.IsNullOrEmpty(customerId))
            {
                return "";
            }

            return customerId;

        }
        catch (Exception ex)
        {
            return "";
        }
    }

    public async Task<ServiceResult<bool>> ChangeMembershipPlan(string customerId, string membershipPlan)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.CustomerId == customerId);
            if (user == null)
                return ServiceResult<bool>.NotFound("Could not find user");

            user.MemberShipPlan = membershipPlan;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return ServiceResult<bool>.Error($"Failed to change membership plan");
            }

            return ServiceResult<bool>.Ok(true);

        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Error("Failed to change membership plan");
        }
    }

    public async Task<ServiceResult<bool>> ChangeSubscriptionStatus(string customerId, string status)
    {
        try
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.CustomerId == customerId);
            if (user == null)
                return ServiceResult<bool>.NotFound("Could not find user");

            user.SubscriptionStatus = status;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return ServiceResult<bool>.Error($"Failed to change subscription status");
            }

            return ServiceResult<bool>.Ok(true);

        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Error("Failed to change subscription status");
        }
    }

    public async Task<ServiceResult<bool>> AddSubscription(string userId, string status, string customerId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ServiceResult<bool>.NotFound("Could not find user");

            user.SubscriptionStatus = status;
            user.CustomerId = customerId;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return ServiceResult<bool>.Error($"Failed to add subscription");
            }

            return ServiceResult<bool>.Ok(true);

        }
        catch (Exception ex)
        {
            return ServiceResult<bool>.Error("Failed to add subscription");
        }
    }

    public async Task<ServiceResult<ProfileModel>> GetProfile(string userId)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ServiceResult<ProfileModel>.NotFound("Could not find user");

            ProfileModel model = new ProfileModel();
            model.UserId = userId;
            model.Email = user.Email!;
            model.MemebershipPlan = user.MemberShipPlan;
            model.SubscriptionStatus = user.SubscriptionStatus;
            if(user.CustomerId != null)
                model.CustomerId = user.CustomerId;

            return ServiceResult<ProfileModel>.Ok(model);

        } catch (Exception ex)
        {
            return ServiceResult<ProfileModel>.Error("Failed to get the profile details");
        }
    }
}
