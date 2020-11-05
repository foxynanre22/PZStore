using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZStore.Models.WorkWithCategories
{
    public static class ProductViewModelHelpers
    {
        public static ProductViewModel ToViewModel(this Product product)
        {
            var productViewModel = new ProductViewModel
            {
                ProductID = product.ProductID,
                Name = product.Name,
                Description = product.Description,
                Image = product.Image,
                Stock = product.Stock,
                Price = product.Price
            };

            return productViewModel;            
        }

        public static Product ToDomainModel (this ProductViewModel productViewModel)
        {
            var product = new Product();

            product.ProductID = productViewModel.ProductID;
            product.Name = productViewModel.Name;
            product.Description = productViewModel.Description;
            product.Image = productViewModel.Image;
            product.Stock = productViewModel.Stock;
            product.Price = productViewModel.Price;
            
            return product;
        }
    }
}