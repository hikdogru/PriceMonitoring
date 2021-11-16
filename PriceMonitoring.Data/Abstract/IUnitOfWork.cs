using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Abstract
    
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IProductPriceRepository ProductPrices { get; }
        IUserRepository Users { get; }
        Task SaveAsync();
        void Save();
    }
}
