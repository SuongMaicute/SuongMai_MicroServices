using Mango.web.Models;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace Mango.web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController( ICartService cart)
        {
            _cartService = cart;
        }
        public async Task<IActionResult> Index()
        {

            return View( await LoadCartFormAPI());
        }


        private async Task<CartDto> LoadCartFormAPI()
        {
            var UserID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;    
            ResponseDto response = await _cartService.GetCartByUserIdAsync(UserID);
            if (response != null && response.IsSuccess) {
                var alo = response.Message;
                CartDto cart = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.result));
                return cart;
            }
            return new CartDto();
        }
    }
}
