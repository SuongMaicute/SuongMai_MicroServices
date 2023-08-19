
using System.ComponentModel.DataAnnotations;

namespace Mango.web.Models
{
    public class CouponDto
    {
        [Required]
        public int CouponId { get; set; }
		[Required]
		public string CouponCode { get; set; }
		
		public double DiscountAmount { get; set; }
		[Required]
		public double MinAmount { get; set; } 
        public DateTime LastUpdate { get; set; }
    }
}
