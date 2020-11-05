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
        public PartialViewResult Register()
        {
            return PartialView();
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

                        return Redirect(Url.Action("CongratsRegister", "Account"));
                    }
                }
                else
                {
                    ModelState.AddModelError("existEmail", "User with the same email is already exist");
                    return PartialView(model);
                }
            }

            return PartialView(model); 
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
        public string ConfirmFalled(string Email)
        {
            if(Email == "")
            {
                return "User with email " + Email + " wasn't registered";
            }
            else
            {
                return "Email " + Email + "wasn't confirmed";
            }       
        }

        [AllowAnonymous]
        public ActionResult CongratsRegister()
        {
            return View("CongratsRegister");
        }

        [AllowAnonymous]
        public PartialViewResult LogIn()
        {
            return PartialView();
        }

        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(LoginModel model)
        {

        }*/

    }
}