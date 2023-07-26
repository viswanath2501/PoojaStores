using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PoojaStores.Models.Repositories.Entity;
using PoojaStores.Models.Repositories.Repositories.Interfaces;
using PoojaStores.Models.Repositories.Repositories.Repos;
using PoojaStores.Models.Services;
using PoojaStores.Models.Services.Interfaces;
using PoojaStores.Models.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PoojaStores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(
              options => options.UseSqlServer(Configuration.GetConnectionString("MyConnection")));

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddMvc().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddScoped<IUsersMgmtRepo, UsersMgmtRepo>();
            services.AddScoped<IMasterMgmtRepo, MasterMgmtRepo>();
            services.AddScoped<ICommonDropsMgmtRepo, CommonDropsMgmtRepo>();
            services.AddScoped<IProductMainMgmtRepo, ProductMainMgmtRepo>();
            services.AddScoped<IImageMgmtRepo, ImageMgmtRepo>();
            services.AddScoped<ISalesMgmtRepo, SalesMgmtRepo>();
            services.AddScoped<IPaymentRepo, PaymentRepo>();
            services.AddScoped<IOrderMgmtRepo, OrderMgmtRepo>();
            services.AddScoped<ICustomerMgmtRepo, CustomerMgmtRepo>();

            services.AddScoped<IUsersMgmtService, UsersMgmtService>();
            services.AddScoped<IMasterMgmtService, MasterMgmtService>();
            services.AddScoped<ICommonDropsMgmtService, CommonDropsMgmtService>();
            services.AddScoped<IProductMgmtService, ProductMgmtService>();
            services.AddScoped<IImageMgmtService, ImageMgmtService>();
            services.AddScoped<ISalesMgmtService, SalesMgmtService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IOrderMgmtService, OrderMgmtService>();
            services.AddScoped<ICustomerMgmtService, CustomerMgmtService>();

            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(160);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();


            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseSession();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
