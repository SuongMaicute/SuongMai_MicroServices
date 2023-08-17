namespace Suongmai.Services.CouponApi.Models.Dto
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }
        public double DiscountAmount { get; set; }
        public string MinAmount { get; set; }
        public DateTime LastUpdate { get; set; }
    }
}
