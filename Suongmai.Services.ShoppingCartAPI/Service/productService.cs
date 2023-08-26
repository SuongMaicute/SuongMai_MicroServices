using Newtonsoft.Json;
using Suongmai.Services.ShoppingCartAPI.Models.Dto;
using Suongmai.Services.ShoppingCartAPI.Service.IService;
using System.Text.Json.Serialization;

namespace Suongmai.Services.ShoppingCartAPI.Service
{
    public class productService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public productService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var response = await client.GetAsync($"api/product/GetAll");
            var apiContent = await response.Content.ReadAsStringAsync();
            var resp = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (resp.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(resp.result));
            }
            return new List<ProductDto>();
        }
    }
}
