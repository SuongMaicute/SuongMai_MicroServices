using Mango.web.Models;
using Mango.web.Service.IService;
using Mango.web.Util;
using static Mango.web.Util.SD;

namespace Mango.web.Service
{
    public class CartService : ICartService
    {
        private readonly IBaseService _baseService;
        public CartService(IBaseService Base)
        {
            _baseService = Base;
        }
        public async Task<ResponseDto> AppyCouponAsync(CartDto cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cart,
                Url = SD.ShoppingCartAPI + "/api/cart/ApplyCoupon"
            });
        }

        public async Task<ResponseDto> GetCartByUserIdAsync(string userID)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = SD.ShoppingCartAPI + "/api/cart/GetCart/"+userID
            });
        }

        public async Task<ResponseDto> RemoveFromCartAsync(int CartDetalId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data=CartDetalId,
                Url = SD.ShoppingCartAPI + "/api/cart/RemoveCart"
            });
        }

        public async Task<ResponseDto> UpsertCartAsync(CartDto cart)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = cart,
                Url = SD.ShoppingCartAPI + "/api/cart/CartUpsert"
            });
        }
    }
}
