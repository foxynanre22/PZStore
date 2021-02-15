using Domain.Abstract;
using PZStore.Models.ContactPage;
using PZStore.SyntaticSugar;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Contact(ContactPageModel model)
        {
            if (ModelState.IsValid)
            {
                string messageSubject = "Message from: " + model.Email;

                string message = string.Format("<html>" +
                "<head>" +
                "<meta charset = \"utf-8\"/>" +
                "</head>" +
                "<body style=\"justify-content: center; align-items: center;\">" +
                "<div style=\"max-width: 640px; margin:0 auto; text-align:center;\">" +
                "<h1>Message from {0}</h1>" +
                "<h5>{1}</h5>" +
                "<hr>" +
                "<h5>Customer waiting for response.</h5>" +
                "</div>" +
                "</body>" +
                "</html>", model.Name, model.Message);

                EmailSender.SendHtmlEmailTo("pzstore9@gmail.com", messageSubject, message);

                return View("ContactThanks");
            }
            else
            {
                return View(model);
            }            
        }
    }
}