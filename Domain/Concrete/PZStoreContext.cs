using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Domain.Concrete
{
    public class PZStoreContext : DbContext
    { 
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //creating many to many relationship between Product and Category
            modelBuilder.Entity<Category>()
                .HasMany<Product>(c => c.Products)
                .WithMany(p => p.Categories)
                .Map(pc =>
                {
                    pc.ToTable("ProductCategory");
                    pc.MapLeftKey("CategoryId");
                    pc.MapRightKey("ProductId");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}