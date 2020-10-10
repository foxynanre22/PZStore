using PZStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PZStore.Controllers
{
    public class HomeController : Controller
    {
        PZStoreContext db = new PZStoreContext();
        public ActionResult Index()
        {
            db.Categories.Add(new Category { Name = "ChildCategory" });
            db.SaveChanges();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}