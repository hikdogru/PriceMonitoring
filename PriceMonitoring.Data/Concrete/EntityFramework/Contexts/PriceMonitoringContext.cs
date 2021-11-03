using Microsoft.EntityFrameworkCore;
using PriceMonitoring.Data.Concrete.EntityFramework.Config;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Concrete.EntityFramework.Contexts
{
    public class PriceMonitoringContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }

        public PriceMonitoringContext(DbContextOptions<PriceMonitoringContext> options) : base(options: options)
        {

        }
        public PriceMonitoringContext()
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new ProductPriceConfiguration());
        }
    }
}
