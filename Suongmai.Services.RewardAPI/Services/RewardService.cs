using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System;
using Suongmai.Services.RewardAPI.Message;
using Suongmai.Services.RewardAPI.Data;
using Suongmai.Services.RewardAPI.Models;

namespace Suongmai.Services.RewardAPI.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<RewardDBContext> _dbOptions;

        public RewardService(DbContextOptions<RewardDBContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task UpdateRewards(RewardsMessage rewardsMessage)
        {
            try
            {
                Reward reward = new()
                {
                    OrderId = rewardsMessage.OrderId,
                    RewardsActivity = rewardsMessage.RewardsActivity,
                    UserId = rewardsMessage.UserId,
                    RewardsDate = DateTime.UtcNow,
                };

                await using var _db = new  RewardDBContext(_dbOptions);
                
                await _db.Rewards.AddAsync(reward);
                await _db.SaveChangesAsync();
               
            }
            catch (Exception ex)
            {

            }
        }
    }
}
