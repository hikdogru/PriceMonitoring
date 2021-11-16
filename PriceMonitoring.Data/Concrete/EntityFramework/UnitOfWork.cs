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
        private EfCoreUserRepository _userRepository;

        public UnitOfWork(PriceMonitoringContext context)
        {
            _context = context;
        }
        public IProductRepository Products => _productRepository = _productRepository ?? new EfCoreProductRepository(_context);
        public IProductPriceRepository ProductPrices => _productPriceRepository = _productPriceRepository ?? new EfCoreProductPriceRepository(_context);
        public IUserRepository Users => _userRepository = _userRepository ?? new EfCoreUserRepository(_context);
        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
