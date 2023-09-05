using Microsoft.EntityFrameworkCore;
using Suongmai.Services.EmailCartAPI.Models;

namespace Suongmai.Services.EmailCartAPI.Data
{
    public class EmailDBContext :DbContext
    {

        public EmailDBContext( DbContextOptions<EmailDBContext> options) : base(options)
        {
            
        }

        public DbSet<EmailLogger> EmailLoggers { get; set; }

    }


}
