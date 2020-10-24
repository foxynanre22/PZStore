using Domain.Abstract;
using Domain.Entities;
using PZStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PZStore.Controllers
{
    public class PartialController : Controller
    {
        private IProductRepository repository;
        public int pageSize = 8;

        public PartialController(IProductRepository repo)
        {
            repository = repo;
        }

        public PartialViewResult ProductCard(Product model)
        {
            return PartialView(model);
        }

        public PartialViewResult ProductsRow(int page = 1)
        {
            ProductsPagesViewModel model = new ProductsPagesViewModel
            {
                Products = repository.Products
                .OrderBy(product => product.ProductID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = repository.Products.Count()
                }
            };

            return PartialView(model);
        }
    }
}