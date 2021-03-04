using Domain.Abstract;
using Domain.Entities;
using PZStore.Models.Authorization;
using PZStore.SyntaticSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PZStore.Controllers
{
    public class AuthorizationController : Controller
    {
        ICustomerRepository customerRepository;

        public AuthorizationController(ICustomerRepository cRepo)
        {
            customerRepository = cRepo;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = null;
                customer = customerRepository.Customers.FirstOrDefault(c => c.Email == model.Email);

                if (customer == null)
                {
                    customerRepository.SaveCustomer(CustomerViewModelHelpers.ToDomainModel(model));

                    customer = customerRepository.Customers.Where(c => c.Email == model.Email && c.Password == model.Password).FirstOrDefault();

                    if (customer != null)
                    {
                        string messageSubject = "Email Confirmation";

                        string content = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Mail Templates/RegisterMailTemplate.html"));

                        string message = string.Format(content, Url.Action("ConfirmEmail", "Authorization", new { Token = customer.CustomerID, Email = customer.Email }, Request.Url.Scheme));

                        EmailSender.SendHtmlEmailTo(customer.Email, messageSubject, message, "C:\\Users\\Daniel Martin\\Desktop\\PZStore\\project\\PZStore\\Assets\\images\\master-page\\logo.png", "logo");

                        return RedirectToAction("CongratsRegister");
                    }
                }
                else
                {
                    ModelState.AddModelError("existEmail", "User with the same email is already exist");
                    return PartialView(model);
                }
            }

            return View(model);
        }

        public ActionResult ConfirmEmail(string Token, string Email)
        {
            Customer user = customerRepository.Customers.FirstOrDefault(c => c.CustomerID == Int32.Parse(Token));

            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.ConfirmedEmail = true;
                    customerRepository.SaveCustomer(user);
                    return View("ConfirmSuccess");
                }
                else
                {
                    return RedirectToAction("ConfirmFalled", "Authorization", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("ConfirmFalled", "Authorization", new { Email = "" });
            }
        }

        public ActionResult ConfirmFalled(string Email)
        {
            if (Email == "")
            {
                ViewBag.Message = "User with email " + Email + " wasn't registered";
                return View();
            }
            else
            {
                ViewBag.Message = "Email " + Email + "wasn't confirmed";
                return View();
            }
        }

        public ActionResult CongratsRegister()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = null;
                customer = customerRepository.Customers.FirstOrDefault(c => c.Email == model.Email);

                if (customer == null)
                {
                    ModelState.AddModelError("unregisteredUser", "User with this email doesn't exist");
                    return View(model);
                }
                else
                {
                    if (model.Email.Contains("@admin.site") && customer.RoleID == 1)
                    {
                        if (customer.Password == model.Password)
                        {
                            FormsAuthentication.SetAuthCookie(customer.Email, true);
                            return RedirectToAction("Index", "Admin");
                        }
                    }
                    else if (customer.ConfirmedEmail)
                    {
                        if (customer.Password == model.Password)
                        {
                            FormsAuthentication.SetAuthCookie(customer.Email, true);
                            Session["Cart"] = null;
                            Session["user"] = customer;
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("wrongPass", "User with this email have another password");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("unconfirmedUser", "User email dont confirm!");
                        return View(model);
                    }

                }
            }

            return View(model);
        }

        public ActionResult Account_Exit()
        {
            FormsAuthentication.SignOut();
            Session["Cart"] = null;
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}