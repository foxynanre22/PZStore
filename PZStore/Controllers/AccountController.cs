using Domain.Abstract;
using Domain.Entities;
using PZStore.Models.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;

namespace PZStore.Controllers
{
    [Authorize (Roles ="user")]
    public class AccountController : Controller
    {
        ICustomerRepository customerRepository;

        public AccountController(ICustomerRepository cRepo)
        {
            customerRepository = cRepo;
        }

        public ActionResult ProfileDetails(string customerEmail)
        {
            Customer customer = customerRepository.Customers.FirstOrDefault(c => c.Email == customerEmail);
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileDetails(Customer model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = customerRepository.Customers.FirstOrDefault(c => c.Email == model.Email);
                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Country = model.Country;
                customer.Password = model.Password;
                customer.Phone = model.Phone;

                customerRepository.SaveCustomer(customer);
                TempData["message"] = string.Format("Detail of \"{0}\" are successful changed", customer.FirstName);

                return RedirectToAction("ProfileDetails", new {customerEmail = customer.Email });
            }
            else
            {
                return View(model);
            }
        }
    }
}