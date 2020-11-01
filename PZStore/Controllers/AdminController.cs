using Domain.Abstract;
using Domain.Entities;
using PZStore.Models.WorkWithCategories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PZStore.Controllers
{
    public class AdminController : Controller
    {
        IProductRepository productRepository;
        ICategoryRepository categoryRepository;

        public AdminController(IProductRepository pRepo, ICategoryRepository cRepo)
        {
            productRepository = pRepo;
            categoryRepository = cRepo;
        }

        public ActionResult Index()
        {
            return View(productRepository.Products);
        }

        public ActionResult Edit(int productID)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productID);

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                productRepository.SaveProduct(product);
                TempData["message"] = string.Format("Product \"{0}\" are successful changed", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

        public ActionResult Create()
        {
            var productViewModel = new ProductViewModel {Categories = FillCategoryData() };
            return View(productViewModel);
        }

        [HttpPost]
        public ActionResult Create(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductID = productViewModel.ProductID,
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Image = productViewModel.Image,
                    Stock = productViewModel.Stock,
                    Price = productViewModel.Price
                };

                AddOrUpdateCategories(product, productViewModel.Categories);
                productRepository.SaveProduct(product);
            }
            return View(productViewModel);
        }

        //for binding Categories and Products (using before saving product to the database)
        private void AddOrUpdateCategories(Product product, ICollection<AssignedCategoryData> assignedCategories)
        {
            if (assignedCategories != null)
            {
                foreach (var assignedCategory in assignedCategories)
                {
                    if (assignedCategory.Assigned)
                    {
                        var category = categoryRepository.Categories.FirstOrDefault(c => c.Name == assignedCategory.Name);
                        product.Categories.Add(category);
                    }
                }
            }
        }

        //for initializing categories in ViewModel in Create() GET Method
        private ICollection<AssignedCategoryData> FillCategoryData()
        {
            var categories = categoryRepository.Categories;
            var assignedCategories = new List<AssignedCategoryData>();

            foreach (var category in categories)
            {
                assignedCategories.Add(new AssignedCategoryData
                {
                    CategoryID = category.CategoryID,
                    Name = category.Name,
                    Assigned = false
                });
            }

            return assignedCategories;
        }
    }
}