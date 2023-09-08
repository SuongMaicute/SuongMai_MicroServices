using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Suongmai.Services.EmailCartAPI.Data;
using Suongmai.Services.EmailCartAPI.Models;
using Suongmai.Services.EmailCartAPI.Models.Dto;
using System.Text;
using System;

namespace Suongmai.Services.EmailCartAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<EmailDBContext> _dbOptions;

        public EmailService(DbContextOptions<EmailDBContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDTO)
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("<br/>Cart Email Requested ");
            message.AppendLine("<br/>Total " + cartDTO.CartHeader.CartTotal);
            message.Append("<br/>");
            message.Append("<ul>");
            foreach (var item in cartDTO.CartDetails)
            {
                message.Append("<li>");
                message.Append(item.Product.Name + " x " + item.Count);
                message.Append("</li>");
            }
            message.Append("</ul>");

            await LogAndEmail(message.ToString(), cartDTO.CartHeader.Email);
        }

        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "User registertration successful. <br/> Email: " + email;
            await LogAndEmail(message, "suongmai@gmail.com");

        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSent = DateTime.Now,
                    Message = message
                };

                await using var _db = new  EmailDBContext(_dbOptions);
                _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
