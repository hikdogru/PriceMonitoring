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
    public class ProductSubscriptionConfiguration : IEntityTypeConfiguration<ProductSubscription>
    {
        public void Configure(EntityTypeBuilder<ProductSubscription> builder)
        {
            builder
                .Property(b => b.UserId)
                .HasMaxLength(5)
                .IsRequired();

            builder
                .Property(b => b.ProductId)
                .HasMaxLength(5)
                .IsRequired();

            builder
                .Property(b => b.ProductPriceId)
                .HasMaxLength(5)
                .IsRequired();

        }
    }
}
