using Suongmai.Services.EmailCartAPI.Models.Dto;

namespace Suongmai.Services.EmailCartAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDTO);
        Task RegisterUserEmailAndLog(string email);
    }
}
