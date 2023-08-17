using System.ComponentModel.DataAnnotations;

namespace Suongmai.Services.CouponApi.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        public double MinAmount { get; set; }
        public DateTime LastUpdate { get; set; }
    }

}
