using Suongmai.Services.ShoppingCartAPI.Models.Dto;

namespace Suongmai.Services.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon( string code);
    }
}
