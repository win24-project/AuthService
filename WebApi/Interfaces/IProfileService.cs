using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IProfileService
    {
        public Task<ServiceResult<ProfileModel>> GetProfile(string userId);
        public Task<ServiceResult<bool>> ChangeMembership(string userId, int membershipId);
    }
}
