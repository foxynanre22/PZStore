using Domain.Abstract;
using Domain.Entities;
using PZStore.Models.WorkWithCategories;
using PZStore.Utility;
using System;
using System.Collections.Generic;
using System.IO;
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

        public ActionResult EditProduct(int productID)
        {
            Product product = productRepository.Products.FirstOrDefault(p => p.ProductID == productID);
            var viewModel = ProductViewModelHelpers.ToViewModel(product);
            viewModel.Categories = FillCategoryData(product);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult EditProduct(ProductViewModel productViewModel, HttpPostedFileBase productImg)
        {
            if (ModelState.IsValid)
            {
                Product product = ProductViewModelHelpers.ToDomainModel(productViewModel);

                //save image on the server and write path to it to the product object
                if (productImg != null)
                {
                    //delete previous image before load the new one
                    string path = Server.MapPath(product.Image);
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    try
                    {
                        product = loadAndBindImage(product, productImg);
                        PZLogger.GetInstance().Info("ADMIN_CONTROLLER::Image for " + product.Name + " has been saved.");
                    }
                    catch (Exception e)
                    {
                        PZLogger.GetInstance().Error("ADMIN_CONTROLLER::Image load error: " + e.Message);
                        return View("~/Views/Shared/Error.cshtml");
                    }
                }

                AddOrUpdateCategories(product, productViewModel.Categories);
                productRepository.SaveProduct(product);
                TempData["message"] = string.Format("Product \"{0}\" are successful changed", product.Name);
            }
            else
            {
                return View(productViewModel);
            }

            return RedirectToAction("Index");
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
        public ActionResult CreateProduct(ProductViewModel productViewModel, HttpPostedFileBase productImg)
        {
            if (ModelState.IsValid)
            {
                Product product = ProductViewModelHelpers.ToDomainModel(productViewModel);

                //save image on the server and write path to it to the product object
                if (productImg != null)
                {
                    try 
                    { 
                        product = loadAndBindImage(product, productImg);
                        PZLogger.GetInstance().Info("ADMIN_CONTROLLER::Image for " + product.Name + " has been saved.");
                    }
                    catch(Exception e) 
                    {
                        PZLogger.GetInstance().Error("ADMIN_CONTROLLER::Image load error: " + e.Message);
                        return View("~/Views/Shared/Error.cshtml");
                    }
                    
                }

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

                //alse delete image of the product from server
                string path = Server.MapPath(product.Image);
                FileInfo file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }


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

        //for initializing categories in ViewModel in Create() and Edit() GET Method
        private ICollection<AssignedCategoryData> FillCategoryData(Product product = null)
        {
            var categories = categoryRepository.Categories;
            var assignedCategories = new List<AssignedCategoryData>();

            if(product != null)
            {
                var productCategories = product.Categories;
                foreach (var category in categories)
                {
                    if (productCategories.Contains(category))
                    {
                        assignedCategories.Add(new AssignedCategoryData
                        {
                            CategoryID = category.CategoryID,
                            Name = category.Name,
                            Assigned = true
                        });
                    }
                    else
                    {
                        assignedCategories.Add(new AssignedCategoryData
                        {
                            CategoryID = category.CategoryID,
                            Name = category.Name,
                            Assigned = false
                        });
                    }                    
                }
            }
            else
            {
                foreach (var category in categories)
                {
                    assignedCategories.Add(new AssignedCategoryData
                    {
                        CategoryID = category.CategoryID,
                        Name = category.Name,
                        Assigned = false
                    });
                }
            }

            return assignedCategories;
        }

        //load image on server + write path to this image in Product object
        private Product loadAndBindImage(Product product, HttpPostedFileBase productImg)
        {
            string fileName = String.Concat(product.Name.Where(c => !Char.IsWhiteSpace(c)));
            string fileExtension = productImg.FileName.Substring(productImg.FileName.IndexOf('.'));
            var fullFileName = Path.GetFileName(fileName + fileExtension);

            var directoryToSave = Server.MapPath(Url.Content("~/Assets/images/products/"));
            var pathToSave = Path.Combine(directoryToSave, fullFileName);

            try
            {
                productImg.SaveAs(pathToSave);
                product.Image = "/Assets/images/products/" + fullFileName;
            }
            catch(Exception e)
            {
                throw e;
            }

            return product;
        }
    }
}