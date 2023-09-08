using Suongmai.Services.OrderAPI.Models.Dto;

namespace Suongmai.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
