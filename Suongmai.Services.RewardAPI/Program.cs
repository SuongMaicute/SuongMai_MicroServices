
using Microsoft.EntityFrameworkCore;
using Suongmai.Services.RewardAPI.Data;
using Suongmai.Services.RewardAPI.Extention;
using Suongmai.Services.RewardAPI.Messaging;
using Suongmai.Services.RewardAPI.Services;

namespace Suongmai.Services.RewardAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<RewardDBContext>(
                option =>
                {
                    option.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
                });

            var optionBuilder = new DbContextOptionsBuilder<RewardDBContext>();
            optionBuilder.UseSqlServer(builder.Configuration.GetConnectionString("MyDB"));
            builder.Services.AddSingleton(new RewardService(optionBuilder.Options));

            builder.Services.AddSingleton<IAzureServiceBusConsumer, AzureServiceBusConsumer>();
            builder.Services.AddScoped<IRewardService, RewardService>();    

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                if (!app.Environment.IsDevelopment())
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "REWARD API");
                    c.RoutePrefix = string.Empty;
                }
            });

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
                    var _db = scope.ServiceProvider.GetRequiredService<RewardDBContext>();

                    if (_db.Database.GetPendingMigrations().Count() > 0)
                    {
                        _db.Database.Migrate();
                    }
                }
            }
        }
    }
}