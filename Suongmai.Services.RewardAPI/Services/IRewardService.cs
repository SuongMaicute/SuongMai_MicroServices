

using Suongmai.Services.RewardAPI.Message;

namespace Suongmai.Services.RewardAPI.Services
{
    public interface IRewardService
    {
        Task UpdateRewards(RewardsMessage rewardsMessage);
    }
}
