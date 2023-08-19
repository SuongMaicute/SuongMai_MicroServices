namespace Suongmai.Services.AuthAPI.Models.Dto
{
    public class ResponseDto
    {
        public object? result { get; set; }
        public bool IsSuccess { get; set; } = true;
        public String Message { get; set; } = "";
    }
}
