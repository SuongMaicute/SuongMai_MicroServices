using Mango.web.Models;

namespace Mango.web.Service.IService
{
	public interface IProductService
	{
		Task<ResponseDto> GetProductAsync(String ProductCode);
		Task<ResponseDto> GetAllProductAsync();
		Task<ResponseDto> GetProductByIdAsync(int id);
		Task<ResponseDto> CreateProductAsync(ProductDto ProductDto);
		Task<ResponseDto> UpdateProductAsync(ProductDto ProductDto);
		Task<ResponseDto> DeleteProductAsync(int id);
	}
}
