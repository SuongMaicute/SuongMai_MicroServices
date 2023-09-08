using Microsoft.EntityFrameworkCore;
using Suongmai.Services.OrderAPI.Models;
using Suongmai.Services.OrderAPI.Models.Dto;

namespace Suongmai.Services.ShoppingCartAPI.Data
{
    public class OrderDBContext :DbContext
    {

        public OrderDBContext( DbContextOptions<OrderDBContext> options) : base(options)
        {
            
        }

        public DbSet<OrderHeader> CarHeOrderHeadersOrderHeadersaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

       


    }


}
