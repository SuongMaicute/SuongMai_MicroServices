using System.ComponentModel.DataAnnotations;

namespace Mango.web.Models
{
    public class LoginResponseDto
    {
        [Required]
        public UserDto User { get; set; }
        [Required]
        public string Token { get; set; }

    }
}
