﻿using Domain.Abstract;
using PZStore.Models.ContactPage;
using PZStore.SyntaticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PZStore.Utility;

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
                
                string content = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Mail Templates/ContactMailTemplate.html"));
                
                // look into file to understand where parameters are
                string message = string.Format(content, model.Name, model.Message);

                try
                {
                    EmailSender.SendHtmlEmailTo("pzstore9@gmail.com", messageSubject, message);
                    PZLogger.GetInstance().Info("HOME_CONTROLLER::Mail from " + model.Email + " successful sent");

                    return View("ContactThanks");

                }
                catch (Exception e)
                {
                    PZLogger.GetInstance().Error("HOME_CONTROLLER::" + e.Message);
                    return View("~/Views/Shared/Error.cshtml");
                }
                
            }
            else
            {
                return View(model);
            }            
        }
    }
}