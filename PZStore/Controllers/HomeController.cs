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
        public ActionResult Index(int productPage = 1)
        {
            ViewBag.productPage = productPage;
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