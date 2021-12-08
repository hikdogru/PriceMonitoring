using BCrypt.Net;
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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
       
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .Property(b => b.Email)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(b => b.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(b => b.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder
                .Property(b => b.Password)
                .HasMaxLength(150)
                .IsRequired();

            builder
                .Property(b => b.IsConfirm)
                .HasMaxLength(1)
                .HasDefaultValue(0)
                .IsRequired();

            builder.HasData(new User
            {
                Id = 1,
                FirstName = "Demo" ,
                LastName = "User",
                Email = "demo@demo.com",
                IsConfirm = true,
                Password = BCrypt.Net.BCrypt.HashPassword("DemoTestPassword123_"),
                Token = ""
            });

        }
    }
}
