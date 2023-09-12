using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;

namespace Mango.web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderServices _orderServices;
        public OrderController( IOrderServices services)
        {
            _orderServices = services;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<OrderHeaderDto> list ;
            string userId = "";
            if (User.IsInRole(SD.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            }
            ResponseDto response = _orderServices.GetAllOrder(userId).GetAwaiter().GetResult();
            if(response != null && response.IsSuccess) 
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.result));

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
    }
}
