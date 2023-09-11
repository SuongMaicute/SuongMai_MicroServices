using Microsoft.EntityFrameworkCore;
using Suongmai.Services.RewardAPI.Models;

namespace Suongmai.Services.RewardAPI.Data
{
    public class RewardDBContext :DbContext
    {

        public RewardDBContext( DbContextOptions<RewardDBContext> options) : base(options)
        {
            
        }

       public DbSet<Reward> Rewards { get; set; }  


    }


}
