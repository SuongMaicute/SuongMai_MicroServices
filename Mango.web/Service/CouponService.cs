using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;
using static Mango.web.Util.SD;

namespace Mango.web.Service
{
    public class CouponService : ICouponService
    {
        private readonly IBaseService _baseService;
        public CouponService( IBaseService baseSV)
        {
            _baseService = baseSV;
        }
        public async Task<ResponseDto> CreateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data =couponDto,
                Url = SD.CouponAPIBase + "api/coupon" 
            });
        }

        public async Task<ResponseDto> DeleteCouponAsync(int id)
        {
            RequestDto request = new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = SD.CouponAPIBase + "api/coupon/" + id
            };
            return await _baseService.SendAsync(request);
        }

        public async Task<ResponseDto> GetAllCouponAsync()
        {
            RequestDto request = new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "api/coupon/GetAll"
            };
            ResponseDto answer = await _baseService.SendAsync(request);
            return answer;
        }

        public async Task<ResponseDto> GetCouponAsync(string couponCode)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "api/coupon/GetByCode/" +couponCode
            });
        }

        public async Task<ResponseDto> GetCouponByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = SD.CouponAPIBase + "api/coupon/" + id
            });
        }

        public async Task<ResponseDto> UpdateCouponAsync(CouponDto couponDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = couponDto,
                Url = SD.CouponAPIBase + "api/coupon"
            });
        }
    }
}
