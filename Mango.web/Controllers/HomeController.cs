using Mango.web.Models;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService service)
        {
            _logger = logger;
            _productService = service;
        }



        public async Task<IActionResult> Index()
        {
            List<ProductDto> products = new List<ProductDto>(); 
            ResponseDto? response = await _productService.GetAllProductAsync();
            if (response != null && response.IsSuccess) {
                products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.result));
            }
            return View(products);
        }

        [Authorize]
        public async Task<IActionResult> ProductDetails( int productID)
        {
            ProductDto product = new ProductDto();
            ResponseDto? respone = await _productService.GetProductByIdAsync(productID);
            if (respone != null  && respone.IsSuccess)
            {
                product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(respone.result));
            }
            else
            {
                TempData["error"] = "Load product fail !!!";
            }
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}