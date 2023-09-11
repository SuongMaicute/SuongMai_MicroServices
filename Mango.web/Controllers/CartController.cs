using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace Mango.web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderServices _orderService;
        public CartController( ICartService cart, IOrderServices _orderServide)
        {
            _cartService = cart;
            _orderService = _orderServide;
        }
        
        [Authorize]
        public async Task<IActionResult> Index()
        {

            return View( await LoadCartFormAPI());
        }

        [Authorize]
        public async Task<IActionResult> Confirmation(int orderID)
        {
            ResponseDto? response = await _orderService.ValidateStripe(orderID);
            var alo = response.Message;
            if (response != null && response.IsSuccess)
            {
                
                OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.result));
                if(orderHeader.Status == SD.Status_Approved)
                {
                    return View(orderID);
                }
                TempData["success"] = "Payment successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(orderID);
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {

            return View(await LoadCartDtoBaseOnLoggedInUser());
        }


        [Authorize]
        [ActionName("CreateOrder")]
        public async Task<IActionResult> CreateOrder(CartDto cartDto)
        {
            CartDto cart = await LoadCartDtoBaseOnLoggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var response = await _orderService.CreateOrderAsync(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.result));

            if (response != null && response.IsSuccess)
            {
                //get stripe session and redirect to stripe to place order
                //
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";

                StripeRequestDto stripeRequestDto = new()
                {
                    ApprovedUrl = domain + "cart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CancelUrl = domain + "cart/checkout",
                    OrderHeader = orderHeaderDto
                };

                var stripeResponse = await _orderService.CreateStripeSession(stripeRequestDto);
                StripeRequestDto stripeResponseResult = JsonConvert.DeserializeObject<StripeRequestDto>
                                            (Convert.ToString(stripeResponse.result));
                Response.Headers.Add("Location", stripeResponseResult.StripeSessionUrl);
                return new StatusCodeResult(303);



            }
            return View();
        }

        public async Task<IActionResult>  Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.RemoveFromCartAsync(cartDetailsId);
            var alo = response.Message;
            if (response != null && response.IsSuccess)
            {

                TempData["success"] = "Remove from cart successfully";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            cartDto.CartDetails = new List<CartDetailDto>();
           ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);
            var alo = response.Message;
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Apply Coupon successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Apply Coupon unsuccessfully";
                return RedirectToAction(nameof(Index));

            }
        }

        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {

            CartDto cart = await LoadCartDtoBaseOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

            ResponseDto? response = await _cartService.EmailCart(cart);
            var alo = response.Message;
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Email will be proceed and sent shortly...";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Send mail unsuccessfully";
                return RedirectToAction(nameof(Index));

            }
        }


        [HttpPost]
        private async Task<CartDto> LoadCartDtoBaseOnLoggedInUser()
        {
            var UserID = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCartByUserIdAsync(UserID);
            if(response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.result));
                return cartDto;
            }
            return new CartDto();
        }



        // remove coupon here 
        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartDetails = new List<CartDetailDto>();
            cartDto.CartHeader.CouponCode = "";
            ResponseDto? response = await _cartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Remove Coupon successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Remove Coupon unsuccessfully";
                return RedirectToAction(nameof(Index));

            }


        }


        private async Task<CartDto> LoadCartFormAPI()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _cartService.GetCartByUserIdAsync(userId);
            var alo = response.Message;
            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.result));
                
                return cartDto;
            }
            return new CartDto();
        }
    }
}
