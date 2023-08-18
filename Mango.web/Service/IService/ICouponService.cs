using Mango.web.Models;

namespace Mango.web.Service.IService
{
    public interface ICouponService
    {
        Task<ResponseDto> GetCouponAsync(String couponCode);
        Task<ResponseDto> GetAllCouponAsync();
        Task<ResponseDto> GetCouponByIdAsync(int id);
        Task<ResponseDto> CreateCouponAsync(CouponDto couponDto);
        Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto);
        Task<ResponseDto> DeleteCouponAsync(int id);


    }
}
