using AutoMapper;
using Suongmai.Services.ProductAPI.Models;
using Suongmai.Services.ProductAPI.Models.Dto;

namespace Suongmai.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>();
            }
                );
            return mappingConfig;
        }
    }
}
