using Mango.web.Models;

namespace Mango.web.Service.IService
{
    public interface IOrderServices
    {
        Task<ResponseDto?> CreateOrderAsync(CartDto cartDto);
        Task<ResponseDto?> CreateStripeSession(StripeRequestDto request);
        Task<ResponseDto?> ValidateStripe(int orderHeaderID);
        Task<ResponseDto?> GetAllOrder(string? UserID);
        Task<ResponseDto?> GetOrder(int orderID);
        Task<ResponseDto?> UpdateOrderStatus(int orderID, string newStatus);



    }
}
