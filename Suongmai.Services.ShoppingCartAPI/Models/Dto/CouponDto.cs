using System.ComponentModel.DataAnnotations;

namespace Suongmai.Services.ShoppingCartAPI.Models.Dto
{
    public class CouponDto
    {
        public int CouponId { get; set; }
       
        public string CouponCode { get; set; }
       
        public double DiscountAmount { get; set; }
        public double MinAmount { get; set; } 
        public DateTime LastUpdate { get; set; }
    }
}
