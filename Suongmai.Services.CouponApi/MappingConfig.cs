using AutoMapper;
using Suongmai.Services.CouponApi.Models;
using Suongmai.Services.CouponApi.Models.Dto;

namespace Suongmai.Services.CouponApi
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CouponDto, Coupon>();
                config.CreateMap<Coupon, CouponDto>();
            }
                );
            return mappingConfig;
        }
    }
}
