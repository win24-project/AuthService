using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IProfileService
    {
        public Task<ServiceResult<ProfileModel>> GetProfile(string userId);

        public Task<ServiceResult<bool>> ChangeMembership(string userId, int membershipId);

        public Task<ServiceResult<bool>> AddSubscription(string userId, string status, string customerId, string membershipId);

        public Task<ServiceResult<bool>> ChangeSubscriptionStatus(string userId, string status);

        public Task<ServiceResult<bool>> ChangeMembershipPlan(string userId, string membershipPlan);

        public Task<string> GetCustomerId(string userId); 

    }
}
