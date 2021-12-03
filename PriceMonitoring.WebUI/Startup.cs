using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Business.Concrete;
using PriceMonitoring.Business.ValidationRules.FluentValidation;
using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Data.Concrete.EntityFramework;
using PriceMonitoring.Data.Concrete.EntityFramework.Contexts;
using PriceMonitoring.WebUI.EmailService;
using PriceMonitoring.WebUI.TimedService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI
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
            // Add dbcontext
            services.AddDbContext<PriceMonitoringContext>(option => option.UseSqlServer(Configuration.GetConnectionString("PriceMonitoringConnectionString"),
                 o => o.CommandTimeout(9999999)));


            // Email configuration
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            // Email sender
            services.AddScoped<IEmailSender, EmailSender>();

            // Dependency Injection
            services.AddScoped<IProductService, ProductManager>();
            services.AddScoped<IProductPriceService, ProductPriceManager>();
            services.AddScoped<IUserService, UserManager>();
            services.AddScoped<IProductSubscriptionService, ProductSubscriptionManager>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            //Automapper
            services.AddAutoMapper(typeof(Startup));

            services.AddHostedService<TimedHostedService>();


            // Session
            services.AddSession(option => option.IdleTimeout = TimeSpan.FromMinutes(20));

            services.AddLogging(option =>
            {
                option.ClearProviders();
                option.AddFile("Logs/pricemonitoring-{Date}.txt");

            });

            services.AddControllersWithViews()
          .AddNewtonsoftJson(options =>
          options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);


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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
