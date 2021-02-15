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
    [Authorize(Roles = "admin")]
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
            var viewModel = ProductViewModelHelpers.ToViewModel(product);
            viewModel.Categories = FillCategoryData();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = ProductViewModelHelpers.ToDomainModel(productViewModel);

                AddOrUpdateCategories(product, productViewModel.Categories);
                productRepository.SaveProduct(product);

                TempData["message"] = string.Format("Product \"{0}\" are successful changed", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                return View(productViewModel);
            }
        }

        public ActionResult Categories()
        {
            return View(categoryRepository.Categories);
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                categoryRepository.SaveCategory(category);
                TempData["message"] = string.Format("Category \"{0}\" are successful created", category.Name);
            }
            else
            {
                return View(category);
            }

            return RedirectToAction("Categories");
        }

        [HttpPost]
        public ActionResult DeleteCategory(int categoryID)
        {
            if (ModelState.IsValid)
            {
                Category category = categoryRepository.Categories.FirstOrDefault(c => c.CategoryID == categoryID);
                categoryRepository.DeleteCategory(category);
                TempData["message"] = string.Format("Category \"{0}\" are successful deleted", category.Name);
            }

            return RedirectToAction("Categories");
        }

        public ActionResult CreateProduct()
        {
            var productViewModel = new ProductViewModel {Categories = FillCategoryData() };
            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var product = ProductViewModelHelpers.ToDomainModel(productViewModel);

                AddOrUpdateCategories(product, productViewModel.Categories);
                productRepository.SaveProduct(product);
                TempData["message"] = string.Format("Product \"{0}\" are successful created", product.Name);
            }
            else
            {
                return View(productViewModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteProduct (int productID)
        {
            if (ModelState.IsValid)
            {
                Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productID);
                productRepository.DeleteProduct(product);
                TempData["message"] = string.Format("Product \"{0}\" are successful deleted", product.Name);
            }

            return RedirectToAction("Index");
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