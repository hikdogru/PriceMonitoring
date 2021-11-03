using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceMonitoring.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Concrete.EntityFramework.Config
{
    public class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
    {
        public void Configure(EntityTypeBuilder<ProductPrice> builder)
        {
            builder
                .Property(b => b.ProductId)
                .HasMaxLength(10)
                .IsRequired();

            builder
                .Property(b => b.Price)
                .HasMaxLength(10)
                .IsRequired();

            builder
                .Property(b => b.SavedDate)
                .HasMaxLength(20)
                .IsRequired();
        }
    }
}
