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
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PriceMonitoring.WebUI.TimedService
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IProductService _productService;
        private IUserService _userService;
        private IProductSubscriptionService _productSubscriptionService;
        private IEmailSender _emailService;
        private IProductPriceService _productPriceService;
        private Timer _timer;

        public TimedHostedService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            var timeDelay = GetTimeDelay();
            _timer = new Timer(DoWork, null, TimeSpan.FromSeconds(timeDelay),
                                             TimeSpan.FromHours(24));
            return Task.CompletedTask;
        }

        private double GetTimeDelay()
        {
            var shouldExecutedTime = DateTime.Parse("17:42");
            var nowTime = DateTime.Parse(DateTime.Now.ToShortTimeString());
            var delayTime = shouldExecutedTime >= nowTime ? (shouldExecutedTime - nowTime).TotalSeconds : new TimeSpan(24, 0, 0).TotalSeconds;
            return delayTime;
        }

        private void DoWork(object state)
        {
            var productsFromMigros = new Migros().GetProducts(url: "https://www.migros.com.tr/meyve-sebze-c-2").ToList();
            SaveProductToDatabaseAndSendEmailToSubscribedUser(products: productsFromMigros);

            var productsFromA101 = new A101().GetProducts(url: "https://www.a101.com.tr/market/meyve-sebze/").ToList();
            SaveProductToDatabaseAndSendEmailToSubscribedUser(productsFromA101);
        }

        private void SetServices()
        {
            using var scope = _scopeFactory.CreateScope();
            _productService = scope.ServiceProvider.GetService<IProductService>();
            _productPriceService = scope.ServiceProvider.GetService<IProductPriceService>();
            _emailService = scope.ServiceProvider.GetService<IEmailSender>();
            _productSubscriptionService = scope.ServiceProvider.GetService<IProductSubscriptionService>();
            _userService = scope.ServiceProvider.GetService<IUserService>();
        }

        private void SaveProductToDatabaseAndSendEmailToSubscribedUser(List<ProductModel> products)
        {
            SetServices();
            var productSubscriptions = _productSubscriptionService.GetAll().Data;
            foreach (var model in products)
            {
                _productService.Add(product: new Product { Image = model.Image, Name = model.Name, WebsiteId = model.WebsiteId });
                var entity = _productService.GetByImageSource(model.Image.ToString());
                var productPrice = new ProductPrice { SavedDate = DateTime.Now, Price = double.Parse(model.Price.Replace("TL", "").Replace(",", ".")), ProductId = entity.Data.Id };
                var subscriptions = productSubscriptions.Where(x => x.ProductId == entity.Data.Id).ToList();
                if (subscriptions.Count() > 0)
                {
                    foreach (var item in subscriptions)
                    {
                        var price = _productPriceService.GetById(item.ProductPriceId).Data;
                        if (price.Price > 0 && productPrice.Price < price.Price)
                        {
                            var user = _userService.GetById(item.UserId).Data;
                            // send message to users
                            if (user is not null)
                            {
                                string reminderFilePath = Directory.GetCurrentDirectory() + @"\Views\Shared\PriceReminderTemplate.cshtml";
                                StreamReader str = new StreamReader(reminderFilePath);
                                string mailText = str.ReadToEnd();
                                mailText = mailText.Replace("{productName}", entity.Data.Name)
                                                    .Replace("{beforePrice}", price.Price.ToString())
                                                    .Replace("{currentPrice}", productPrice.Price.ToString());
                                var message = new Message(to: user.Email, subject: "Product price drop",
                                                              content: mailText);
                                _emailService.SendEmail(message: message);
                            }
                        }
                    }
                }
                _productPriceService.Add(productPrice: productPrice);
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
