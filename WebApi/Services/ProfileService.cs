using Microsoft.AspNetCore.Identity;
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
            model.MembershipId = user.MembershipId;

            return ServiceResult<ProfileModel>.Ok(model);

        } catch (Exception ex)
        {
            return ServiceResult<ProfileModel>.Error("Failed to get the profile details");
        }
    }
}
