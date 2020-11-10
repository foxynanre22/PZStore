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

        public PartialViewResult ProductsRow(string category, int page = 1)
        {
            ProductsPagesViewModel model = new ProductsPagesViewModel
            {
                Products = repository.Products
                .Where(p => category == null || p.Categories.FirstOrDefault().Name == category)
                .OrderBy(product => product.ProductID)
                .Skip((page - 1) * pageSize)
                .Take(pageSize),

                /*for PageLinks helper on View*/
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    TotalItems = category == null ?
                        repository.Products.Count() : repository.Products.Where(p => p.Categories.FirstOrDefault().Name == category).Count()
                },

                CurrentCategory = category
            }; 

            return PartialView(model);
        }

        public ActionResult ProductFullView(int productID)
        {
            Product current_product = repository.Products.FirstOrDefault(p => p.ProductID == productID);
            return View(current_product);
        }
    }
}