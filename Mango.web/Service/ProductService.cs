using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;
using static Mango.web.Util.SD;

namespace Mango.web.Service
{
	public class ProductService : IProductService
	{
		private readonly IBaseService _baseService;
		public ProductService(IBaseService baseSV)
		{
			_baseService = baseSV;
		}
		public async Task<ResponseDto> CreateProductAsync(ProductDto ProductDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = ApiType.POST,
				Data = ProductDto,
				Url = SD.ProductAPIBase + "/api/product",
				ContentType = SD.ContentType.MultipartFormData
			});
		}

		public async Task<ResponseDto> DeleteProductAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = ApiType.DELETE,
				Url = SD.ProductAPIBase + "/api/product/" + id
			});
		}

		public async Task<ResponseDto> GetAllProductAsync()
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = SD.ApiType.GET,
				Url = SD.ProductAPIBase + "/api/product/GetAll"
			});


		}

		public async Task<ResponseDto> GetProductAsync(string ProductCode)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = ApiType.GET,
				Url = SD.ProductAPIBase + "/api/product/GetByName/" + ProductCode
			});
		}

		public async Task<ResponseDto> GetProductByIdAsync(int id)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = ApiType.GET,
				Url = SD.ProductAPIBase + "/api/product/" + id
			});
		}

		public async Task<ResponseDto> UpdateProductAsync(ProductDto ProductDto)
		{
			return await _baseService.SendAsync(new RequestDto()
			{
				ApiType = ApiType.PUT,
				Data = ProductDto,
				Url = SD.ProductAPIBase + "/api/product",
				ContentType = SD.ContentType.MultipartFormData
			});
		}
	}
}
