using AutoMapper;
using Suongmai.Services.ShoppingCartAPI.Models;
using Suongmai.Services.ShoppingCartAPI.Models.Dto;
using Suongmai.Services.ShoppingCartAPI.Models;
using Suongmai.Services.ShoppingCartAPI.Models.Dto;

namespace Suongmai.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap();
                config.CreateMap<CartDetail, CartDetailDto>().ReverseMap();
            }
                );
            return mappingConfig;
        }
    }
}
