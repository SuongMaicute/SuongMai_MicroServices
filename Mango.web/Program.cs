using Mango.web.Service;
using Mango.web.Service.IService;
using Mango.web.Util;

namespace Mango.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // configuration this play a client role to send request to api service project
            builder.Services.AddControllersWithViews();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient();
            builder.Services.AddHttpClient<ICouponService,CouponService>();

            SD.CouponAPIBase = builder.Configuration["ServiceUrls:CouponAPI"];

            // add service into each request
            builder.Services.AddScoped<IBaseService, BaseService>();
            builder.Services.AddScoped<ICouponService, CouponService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}