﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public class ProductContext:DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options):base(options)
        {

        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().Property(x => x.Price).HasColumnType("decimal(18,2)");
            modelBuilder.Entity<Product>().HasData(new Product[]
            {
                new Product{Id=1,Name="Bilgisayar",CreatedDate=DateTime.Now.AddDays(-3),Price=15000,Stock=300},
                new Product{Id=2,Name="Telefon",CreatedDate=DateTime.Now.AddDays(-30),Price=10000,Stock=500},
                new Product{Id=3,Name="Klavye",CreatedDate=DateTime.Now.AddDays(-60),Price=500,Stock=400}
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
