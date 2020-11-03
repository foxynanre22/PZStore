using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class ProductRepository : IProductRepository
    {
        PZStoreContext context = MainContext.context;
<<<<<<< HEAD
=======
>>>>>>> Admin
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }

        public void SaveProduct(Product product)
        {
            if(product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbProduct = context.Products.Find(product.ProductID);

                if(dbProduct != null)
                {
                    dbProduct.Name = product.Name;
                    dbProduct.Description = product.Description;
                    dbProduct.Price = product.Price;
                    dbProduct.Stock = product.Stock;
                    dbProduct.Image = product.Image;
                    dbProduct.Categories = product.Categories;
                }
            }

            context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
            if (product.ProductID != 0)
            {
                Product dbProduct = context.Products.Find(product.ProductID);

                if (dbProduct != null)
                {
                    context.Products.Remove(dbProduct);
                    context.SaveChanges();
                }
            }  
        }
    }
}
