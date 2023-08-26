using Suongmai.Services.ShoppingCartAPI.Models.Dto;

namespace Suongmai.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
