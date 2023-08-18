using static Mango.web.Util.SD;

namespace Mango.web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public String Url { get; set; } 
        public object? Data { get; set; }
        public String? AccessToken { get; set; }
    }
}
