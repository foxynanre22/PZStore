using Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PZStore.Controllers
{
    public class HomeController : Controller
    {
        /*productPage parameter is send by ProductsRow*/
        public ActionResult Index(string category = null, int productPage = 1)
        {
            ViewBag.productPage = productPage;
            ViewBag.currentCategory = category;
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}