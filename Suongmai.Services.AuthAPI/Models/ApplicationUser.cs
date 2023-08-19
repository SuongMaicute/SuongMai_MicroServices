using Microsoft.AspNetCore.Identity;

namespace Suongmai.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

    }
}
