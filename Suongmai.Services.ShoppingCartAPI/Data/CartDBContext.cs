using Microsoft.EntityFrameworkCore;
using Suongmai.Services.ShoppingCartAPI.Models;
using Suongmai.Services.ShoppingCartAPI.Models.Dto;

namespace Suongmai.Services.ShoppingCartAPI.Data
{
    public class CartDBContext :DbContext
    {

        public CartDBContext( DbContextOptions<CartDBContext> options) : base(options)
        {
            
        }

        public DbSet<CartHeader> CarHeaders { get; set; }
        public DbSet<CartDetail> CarDeatails { get; set; }

       
		

    }


}
