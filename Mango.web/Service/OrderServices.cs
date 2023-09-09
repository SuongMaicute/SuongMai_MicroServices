using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;

namespace Mango.web.Service
{
    public class OrderServices : IOrderServices
    {
        private readonly IBaseService _baseService;
        public OrderServices(IBaseService Base)
        {
            _baseService = Base;
        }
        public async Task<ResponseDto> CreateOrderAsync(CartDto cartDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.OrderAPIBase + "/api/order/CreateOrder"
            });
        }

        public async Task<ResponseDto> CreateStripeSession(StripeRequestDto request)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = request,
                Url = SD.OrderAPIBase + "/api/order/CreateStripeSession"
            });
        }

        public async Task<ResponseDto?> ValidateStripe(int orderHeaderID)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = orderHeaderID,
                Url = SD.OrderAPIBase + "/api/order/ValidateStripe"
            });
        }
    }
}
