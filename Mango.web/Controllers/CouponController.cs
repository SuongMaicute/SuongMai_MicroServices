using Mango.web.Models;
using Mango.web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController( ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto> list = null;
            ResponseDto? response = await _couponService.GetAllCouponAsync();
             if (response != null && response.IsSuccess) {
				list = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.result));
				

			}
            else
            {
                TempData["error"] = response.Message;
            }
            return View(list);
        }

		public async Task<IActionResult> CouponCreate()
		{
            CouponDto coupon = new CouponDto();	
			return View(coupon);
		}
		[HttpPost]
		public async Task<IActionResult> CouponCreate(CouponDto coupon)
		{
			if(ModelState.IsValid)
			{
				ResponseDto? response = await _couponService.CreateCouponAsync(coupon);
				if (response != null && response.IsSuccess)
				{
                    TempData["success"] = "Create Coupon successfully";
					return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }
			 return View(coupon); 
		}

	
		public async Task<IActionResult> CouponDelete(int couponId)
		{
			ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);
			if (response != null && response.IsSuccess)
			{
				CouponDto? coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.result));
				return View(coupon);

			}
            else
            {
                TempData["error"] = response.Message;
            }
            return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> CouponDelete(CouponDto coupon)
		{
			
				ResponseDto? response = await _couponService.DeleteCouponAsync(coupon.CouponId);
				if (response != null && response.IsSuccess)
				{
					TempData["success"] = "Delete Coupon successfully";
					return RedirectToAction(nameof(CouponIndex));

            }
			else
			{
				TempData["error"] = response.Message;
			}
			
			return View(coupon);
		}

	}
}
