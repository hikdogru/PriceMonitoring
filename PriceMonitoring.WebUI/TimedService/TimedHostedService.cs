using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.WebUI.EmailService;
using PriceMonitoring.WebUI.Models;
using PriceMonitoring.WebUI.Models.GroceryStore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.TimedService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        public TimedHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            //var timeDelay = GetTimeDelay();
            //_timer = new Timer(DoWork, null, TimeSpan.FromSeconds(timeDelay),
            //                                 TimeSpan.FromHours(24));
            return Task.CompletedTask;
        }

        private double GetTimeDelay()
        {
            var shouldExecutedTime = TimeOnly.Parse("15:43");
            var nowTime = TimeOnly.FromDateTime(DateTime.Now);
            var timeDelay = (shouldExecutedTime - nowTime).TotalSeconds;
            return timeDelay;
        }

        private void DoWork(object state)
        {
            var productsFromMigros = new Migros().GetProducts(url: "https://www.migros.com.tr/meyve-sebze-c-2").ToList();
            SaveDatabase(products: productsFromMigros);

            var productsFromA101 = new A101().GetProducts(url: "https://www.a101.com.tr/market/meyve-sebze/").ToList();
            SaveDatabase(productsFromA101);
        }

        private void SaveDatabase(List<ProductModel> products)
        {
            using var scope = _scopeFactory.CreateScope();
            var productService = scope.ServiceProvider.GetService<IProductService>();
            var productPriceService = scope.ServiceProvider.GetService<IProductPriceService>();
            var emailService = scope.ServiceProvider.GetService<IEmailSender>();
            var productSubscriptionService = scope.ServiceProvider.GetService<IProductSubscriptionService>();
            var userService = scope.ServiceProvider.GetService<IUserService>();
            var productSubscriptions = productSubscriptionService.GetAll().Data;
            foreach (var model in products)
            {
                productService.Add(product: new Product { Image = model.Image, Name = model.Name, WebsiteId = model.WebsiteId });
                var entity = productService.GetByImageSource(model.Image.ToString());
                var productPrice = new ProductPrice { SavedDate = DateTime.Now, Price = double.Parse(model.Price.Replace("TL", "").Replace(",", ".")), ProductId = entity.Data.Id };
                var subscriptions = productSubscriptions.Where(x => x.ProductId == entity.Data.Id).ToList();
                if (subscriptions.Count() > 0)
                {
                    foreach (var item in subscriptions)
                    {
                        var price = productPriceService.GetById(item.ProductPriceId).Data;
                        if (price.Price > 0 && productPrice.Price < price.Price)
                        {
                            var user = userService.GetById(item.UserId).Data;
                            // send message to users
                            if (user is not null)
                            {
                                var message = new Message(to: user.Email, subject: "Product price drop",
                                                              content: $"<div style='margin:10px 10px;'> <h3 style='color:green;'>" +
                                                              $" The price of {entity.Data.Name} has dropped." +
                                                              $" <br> The price when you subscribed: {price.Price} TRY " +
                                                              $" <br> Current price : {productPrice.Price} TRY  </h3> </div>");
                                emailService.SendEmail(message: message);
                            }
                        }
                    }
                }
                productPriceService.Add(productPrice: productPrice);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
