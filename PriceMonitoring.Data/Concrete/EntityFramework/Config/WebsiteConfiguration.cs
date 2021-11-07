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
    public class WebsiteConfiguration : IEntityTypeConfiguration<Website>
    {
        public void Configure(EntityTypeBuilder<Website> builder)
        {
            builder.HasKey(x => x.Id);

            builder
                .Property(b => b.Name)
                .HasMaxLength(50)
                .ValueGeneratedOnAdd()
                .IsRequired();


            builder.HasData(new Website {Id = 1, Name = "Migros"}, new Website {Id = 2, Name = "A101"});
        }
    }
}
