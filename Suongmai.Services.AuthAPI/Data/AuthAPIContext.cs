using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Suongmai.Services.AuthAPI.Models;

namespace Suongmai.Services.AuthAPI.Data
{
    public class AuthAPIContext :IdentityDbContext <ApplicationUser>
    {

        public AuthAPIContext( DbContextOptions<AuthAPIContext> options) : base(options)
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }


}
