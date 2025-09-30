using WebApi.Models;

namespace WebApi.Interfaces
{
    public interface IProfileService
    {
        public Task<ServiceResult<ProfileModel>> GetProfile(string userId);

        public Task<ServiceResult<bool>> ChangeMembership(string userId, int membershipId);

        public Task<ServiceResult<bool>> AddSubscription(string userId, string status, string customerId);

        public Task<ServiceResult<bool>> ChangeSubscriptionStatus(string customerId, string status);

        public Task<ServiceResult<bool>> ChangeMembershipPlan(string customerId, string membershipPlan);

        public Task<string> GetCustomerId(string userId); 

    }
}
