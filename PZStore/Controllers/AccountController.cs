using Domain.Abstract;
using Domain.Entities;
using PZStore.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;

namespace PZStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        ICustomerRepository customerRepository;

        public AccountController(ICustomerRepository cRepo)
        {
            customerRepository = cRepo;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = null;
                customer = customerRepository.Customers.FirstOrDefault(c => c.Email == model.Email);

                if(customer == null)
                {
                    customerRepository.SaveCustomer(CustomerViewModelHelpers.ToDomainModel(model));

                    customer = customerRepository.Customers.Where(c => c.Email == model.Email && c.Password == model.Password).FirstOrDefault();

                    if (customer != null)
                    {
                        MailAddress from = new MailAddress("pzstore9@gmail.com", "PZStore Registration");
                        MailAddress to = new MailAddress(customer.Email);
                        MailMessage message = new MailMessage(from, to);

                        message.Subject = "PZStore email confirmation";
                        message.Body = string.Format("For successful end of registration follow this link:" +
                                        "<a href=\"{0}\" title=\"Confirm Email\">{0}</a>",
                            Url.Action("ConfirmEmail", "Account", new { Token = customer.CustomerID, Email = customer.Email }, Request.Url.Scheme));
                        message.IsBodyHtml = true;

                        SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = true;
                        smtp.EnableSsl = true;
                        smtp.Credentials = new System.Net.NetworkCredential("pzstore9@gmail.com", "lfyybk546J@");
                        
                        smtp.Send(message);

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

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string Token, string Email)
        {
            Customer user = customerRepository.Customers.FirstOrDefault(c => c.CustomerID == Int32.Parse(Token));

            if (user != null)
            {
                if (user.Email == Email)
                {
                    user.ConfirmedEmail = true;
                    customerRepository.SaveCustomer(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("ConfirmFalled", "Account", new { Email = user.Email });
                }
            }
            else
            {
                return RedirectToAction("ConfirmFalled", "Account", new { Email = "" });
            }
        }

        [AllowAnonymous]
        public ActionResult ConfirmFalled(string Email)
        {
            if(Email == "")
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

        [AllowAnonymous]
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
        [AllowAnonymous]
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
                    if (customer.ConfirmedEmail)
                    {
                        if (customer.Password == model.Password)
                        {
                            FormsAuthentication.SetAuthCookie(customer.Email, true);
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
            return RedirectToAction("Index", "Home");
        }

    }
}