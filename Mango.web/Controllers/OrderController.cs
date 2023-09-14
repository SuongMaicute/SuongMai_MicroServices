using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Mango.web.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        public OrderController( IOrderServices services)
        {
            _orderServices = services;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> orderDetail(int orderId)
        {
            OrderHeaderDto orderheaderDto = new OrderHeaderDto();
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var respone = await _orderServices.GetOrder(orderId);
            if (respone != null && respone.IsSuccess) {
                orderheaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(respone.result));

            }

            if (!User.IsInRole(SD.RoleAdmin) && userId!= orderheaderDto.UserId)
            {
                return NotFound();
            }
            return View(orderheaderDto);

        }

		[HttpGet]
		public IActionResult GetAll(string status)
		{
            IEnumerable<OrderHeaderDto> list ;
            string userId = "";
            
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            
            ResponseDto response = _orderServices.GetAllOrder(userId).GetAwaiter().GetResult();
            if(response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.result));
                switch(status)
                {
                    case "approved":
                        list = list.Where(u =>u.Status == SD.Status_Approved).ToList(); break;
					case "readyforpickup":
						list = list.Where(u => u.Status == SD.Status_ReadyForPickup).ToList(); break;
					case "cancelled":
						list = list.Where(u => u.Status == SD.Status_Cancelled).ToList(); break;
                    default:
					    break;

				}
            }
            else
            {
                list = new List<OrderHeaderDto>();
            }
            return Json(new
            {
                data = list
            } );


        }

        [HttpPost("OrderReadyForPickup")]
        public async Task<IActionResult> OrderReadyForPickup(int orderID)
        {
            ResponseDto response = await _orderServices.UpdateOrderStatus(orderID, SD.Status_ReadyForPickup);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status update succesfully ";
                
            }
            return RedirectToAction(nameof(orderDetail), new { orderId = orderID });

        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderID)
        {
            ResponseDto response = await _orderServices.UpdateOrderStatus(orderID, SD.Status_Cancelled);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status update succesfully ";
            }
            return RedirectToAction(nameof(orderDetail), new { orderId = orderID });
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderID)
        {
            ResponseDto response = await _orderServices.UpdateOrderStatus(orderID, SD.Status_Completed);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status update succesfully ";
            }
            return RedirectToAction(nameof(orderDetail), new { orderId = orderID });
        }

    }
}
