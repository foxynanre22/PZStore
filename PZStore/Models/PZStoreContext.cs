using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PZStore.Models
{
    public class PZStoreContext : DbContext
    { 
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //creating many to many relationship between Product and Category
            modelBuilder.Entity<Category>()
                .HasMany<Product>(c => c.Products)
                .WithMany(p => p.Categories)
                .Map(pc =>
                {
                    pc.ToTable("ProductCategory");
                    pc.MapLeftKey("ProductId");
                    pc.MapRightKey("CategoryId");
                });

            //creating many to many relationship between Product and Order
            modelBuilder.Entity<Order>()
                .HasMany<Product>(o => o.Products)
                .WithMany(p => p.Orders)
                .Map(op =>
                {
                    op.ToTable("OrderDetails");
                    op.MapLeftKey("OrderId");
                    op.MapRightKey("ProductId"); 
                });

            base.OnModelCreating(modelBuilder);
        }

    }
}