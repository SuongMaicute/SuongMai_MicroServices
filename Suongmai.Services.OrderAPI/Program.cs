
using AutoMapper;
using Mango.MessageBus;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Stripe;
using Suongmai.Services.OrderAPI.Extentions;
using Suongmai.Services.OrderAPI.RabbitMQSender;
using Suongmai.Services.OrderAPI.Service;
using Suongmai.Services.OrderAPI.Service.IService;
using Suongmai.Services.OrderAPI.Util;
using Suongmai.Services.ShoppingCartAPI.Data;

namespace Suongmai.Services.OrderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<OrderDBContext>(
                option =>
                {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
                });
            // add auto mapper to our services 
            IMapper mapper = MappingConfig.RegisterMaps().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddScoped<IProductService, productService>();
            builder.Services.AddScoped<IRabbbitIMOrderMessageSender, RabbbitIMOrderMessageSender>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<SuongMaiAuthenticationHandler>();
            builder.Services.AddHttpClient("Product", u => u.BaseAddress =
            new Uri(builder.Configuration["ServiceUrl:ProductAPI"])).AddHttpMessageHandler<SuongMaiAuthenticationHandler>();
           builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme, securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter string as follow : Bearer Generated-JWT-Token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    }, new string[]{}
                    }
                });
            }
            );

            builder.AppAuthentication();
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (!app.Environment.IsDevelopment())
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Order API");
                    c.RoutePrefix = string.Empty;
                }
            });
            StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:secretKey").Get<string>();

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            ApplyMigration();
            app.Run();

            void ApplyMigration()
            {
                using (var scope = app.Services.CreateScope())
                {
                    var _db = scope.ServiceProvider.GetRequiredService<OrderDBContext>();

                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();
                    }
                }
            }
        }
    }
}