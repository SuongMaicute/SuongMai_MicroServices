
using Mango.MessageBus;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Suongmai.Services.AuthAPI.Data;
using Suongmai.Services.AuthAPI.Models;
using Suongmai.Services.AuthAPI.Service;
using Suongmai.Services.AuthAPI.Service.IService;
using System;

namespace Suongmai.Services.AuthAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AuthAPIContext>(
                option =>
                {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
                });
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Apisettings:JwtOptions"));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthAPIContext>()
    .AddDefaultTokenProviders();

            builder.Services.AddControllers();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IMessageBus, MessageBus>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (!app.Environment.IsDevelopment())
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AUTH API");
                    c.RoutePrefix = string.Empty;
                }
            });
            // }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();
            ApplyMigration();

            app.Run();

            void ApplyMigration()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<AuthAPIContext>();

                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();
                    }
                }
            }

        }
    }
}