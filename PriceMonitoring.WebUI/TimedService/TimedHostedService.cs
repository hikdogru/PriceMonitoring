using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PriceMonitoring.Business.Abstract;
using PriceMonitoring.Entities.Concrete;
using PriceMonitoring.WebUI.Models;
using PriceMonitoring.WebUI.Models.GroceryStore;
using System;
using System.Collections.Generic;
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
            if (DateTime.Now.Hour == 17 && DateTime.Now.Minute == 18)
            {
                _timer = new Timer(DoWork, null, TimeSpan.Zero,
                                TimeSpan.FromHours(24));
            }


            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            Console.WriteLine("Hello");

            var productsFromMigros = new Migros().GetProducts(url: "https://www.migros.com.tr/meyve-sebze-c-2").ToList();
            SaveDatabase(products: productsFromMigros);

            var productsFromA101 = new A101().GetProducts(url: "https://www.a101.com.tr/market/meyve-sebze/").ToList();
            SaveDatabase(productsFromA101);

        }

        private void SaveDatabase(List<ProductModel> products)
        {
            foreach (var model in products)
            {
                using var scope = _scopeFactory.CreateScope();
                var _productService = scope.ServiceProvider.GetService<IProductService>();
                var _productPriceService = scope.ServiceProvider.GetService<IProductPriceService>();
                _productService.Add(product: new Product { Image = model.Image, Name = model.Name, WebsiteId = model.WebsiteId });
                var entity = _productService.GetByImageSource(model.Image.ToString());
                var productPrice = new ProductPrice { SavedDate = DateTime.Now, Price = double.Parse(model.Price.Replace("TL", "").Replace(",", ".")), ProductId = entity.Data.Id };
                _productPriceService.Add(productPrice: productPrice);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
