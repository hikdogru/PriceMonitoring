﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PriceMonitoring.Data.Concrete.EntityFramework.Contexts;

namespace PriceMonitoring.Data.Migrations
{
    [DbContext(typeof(PriceMonitoringContext))]
    partial class PriceMonitoringContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("WebsiteId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WebsiteId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.ProductPrice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Price")
                        .HasMaxLength(10)
                        .HasColumnType("float");

                    b.Property<int>("ProductId")
                        .HasMaxLength(10)
                        .HasColumnType("int");

                    b.Property<DateTime>("SavedDate")
                        .HasMaxLength(20)
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPrices");
                });

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.ProductSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProductId")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int>("ProductPriceId")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ProductSubscriptions");
                });

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsConfirm")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.Website", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Websites");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Migros"
                        },
                        new
                        {
                            Id = 2,
                            Name = "A101"
                        });
                });

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.Product", b =>
                {
                    b.HasOne("PriceMonitoring.Entities.Concrete.Website", "Website")
                        .WithMany("Products")
                        .HasForeignKey("WebsiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Website");
                });

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.ProductPrice", b =>
                {
                    b.HasOne("PriceMonitoring.Entities.Concrete.Product", "Product")
                        .WithMany("ProductPrice")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.Product", b =>
                {
                    b.Navigation("ProductPrice");
                });

            modelBuilder.Entity("PriceMonitoring.Entities.Concrete.Website", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
