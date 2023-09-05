
using Microsoft.EntityFrameworkCore;
using Suongmai.Services.EmailCartAPI.Data;
using Suongmai.Services.EmailCartAPI.Extention;
using Suongmai.Services.EmailCartAPI.Messaging;
using Suongmai.Services.EmailCartAPI.Services;
using System;

namespace Suongmai.Services.EmailCartAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<EmailDBContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
            });
            var optionBuilder = new DbContextOptionsBuilder<EmailDBContext>();
            optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
            builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

            builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            ApplyMigration();
            app.UseAzureServiceBusConsumer();
            app.Run();

            void ApplyMigration()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<EmailDBContext>();
                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();
                    }
                }
            }
        }
    }
}