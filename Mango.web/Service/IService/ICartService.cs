using Mango.web.Models;
using System.Threading.Tasks;

namespace Mango.web.Service.IService
{
    public interface ICartService
    {
        Task<ResponseDto> GetCartByUserIdAsync(string userID);
        Task<ResponseDto> UpsertCartAsync(CartDto cart);
        Task<ResponseDto> ApplyCouponAsync( CartDto cart);
        Task<ResponseDto> RemoveFromCartAsync(int  CartDetalId);

        Task<ResponseDto> EmailCart(CartDto cartDto);
    }
}
