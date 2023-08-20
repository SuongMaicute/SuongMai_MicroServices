using Suongmai.Services.AuthAPI.Models.Dto;

namespace Suongmai.Services.AuthAPI.Service.IService
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssingRole(String email, string roleName);
    }
}
