using Mango.web.Models;
using Mango.web.Service;
using Mango.web.Service.IService;
using Mango.web.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;

namespace Mango.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService Service)
        {
            _productService = Service;
        }
        public async Task<IActionResult> IndexAsync()
        {
            List<ProductDto> list = null;
            ResponseDto? response = await _productService.GetAllProductAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.result));
            }
            else
            {
                TempData["error"] = response.Message;
            }
            return View(list);
        }



       
        public async Task<IActionResult> CreateProduct()
        {
            ProductDto coupon = new ProductDto();
            return View(coupon);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto product)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(product);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Create Coupon successfully";
                    return RedirectToAction(nameof(IndexAsync));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
            return View(product);
        }


        public async Task<IActionResult> DeleteProduct(int productID)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(productID);
            if (response != null && response.IsSuccess)
            {
                ProductDto? product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.result));
                return View(product);

            }
            else
            {
                TempData["error"] = response.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductDto product)
        {

            ResponseDto? response = await _productService.DeleteProductAsync(product.ProductId);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Delete Coupon successfully";
                return RedirectToAction(nameof(IndexAsync));

            }
            else
            {
                TempData["error"] = response.Message;
            }

            return View(product);
        }

    }

}

