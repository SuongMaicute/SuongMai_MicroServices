using Mango.web.Models;

namespace Mango.web.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto);


    }
}
