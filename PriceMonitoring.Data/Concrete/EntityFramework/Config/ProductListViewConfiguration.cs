using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PriceMonitoring.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceMonitoring.Data.Concrete.EntityFramework.Config
{
    public class ProductListViewConfiguration : IEntityTypeConfiguration<ProductListDto>
    {
        public void Configure(EntityTypeBuilder<ProductListDto> builder)
        {
            builder.HasNoKey();
            builder.ToView(name: "ProductList_View");
        }
    }
}
