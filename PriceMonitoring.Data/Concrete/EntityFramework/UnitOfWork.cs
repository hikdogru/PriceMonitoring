using PriceMonitoring.Data.Abstract;
using PriceMonitoring.Data.Concrete.EntityFramework.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Concrete.EntityFramework
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly PriceMonitoringContext _context;
        private EfCoreProductRepository _productRepository;
        private EfCoreProductPriceRepository _productPriceRepository;

        public IProductRepository Products => _productRepository = _productRepository ?? new EfCoreProductRepository();
        public IProductPriceRepository ProductPrices => _productPriceRepository = _productPriceRepository ?? new EfCoreProductPriceRepository();

        public UnitOfWork(PriceMonitoringContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
