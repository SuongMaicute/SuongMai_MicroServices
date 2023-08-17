using Microsoft.EntityFrameworkCore;
using Suongmai.Services.CouponApi.Models;

namespace Suongmai.Services.CouponApi.Data
{
    public class AppDBContext :DbContext
    {

        public AppDBContext( DbContextOptions<AppDBContext> options) : base(options)
        {
            
        }

        public DbSet<Coupon> Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Coupon>().HasData(
                new Coupon
                {
                    CouponId = 1,
                    CouponCode = "10Off",
                    DiscountAmount=10,
                    MinAmount = 20
                }, new Coupon
                {
                    CouponId = 2,
                    CouponCode = "11Off",
                    DiscountAmount = 10,
                    MinAmount = 20
                }
                );
        }

    }


}
