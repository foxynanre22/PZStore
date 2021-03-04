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
using System.IO;

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
        public ActionResult ProfileDetails(Customer model, HttpPostedFileBase userImg)
        {
            if (ModelState.IsValid)
            {
                Customer customer = customerRepository.Customers.FirstOrDefault(c => c.Email == model.Email);
                customer.FirstName = model.FirstName;
                customer.LastName = model.LastName;
                customer.Country = model.Country;
                customer.Password = model.Password;
                customer.Phone = model.Phone;

                if (userImg != null)
                {
                    //delete previous image before load the new one
                    string path = Server.MapPath(customer.Image);
                    FileInfo file = new FileInfo(path);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    customer = loadAndBindImage(customer, userImg);
                }

                customerRepository.SaveCustomer(customer);
                TempData["message"] = string.Format("Detail of \"{0}\" are successful changed", customer.FirstName);

                return RedirectToAction("ProfileDetails", new {customerEmail = customer.Email });
            }
            else
            {
                return View(model);
            }
        }

        //load image on server + write path to this image in Product object
        private Customer loadAndBindImage(Customer customer, HttpPostedFileBase userImg)
        {
            string fileName = customer.FirstName + customer.LastName + customer.CustomerID;
            string fileExtension = userImg.FileName.Substring(userImg.FileName.IndexOf('.'));
            var fullFileName = Path.GetFileName(fileName + fileExtension);

            var directoryToSave = Server.MapPath(Url.Content("~/Assets/images/users-photo/"));
            var pathToSave = Path.Combine(directoryToSave, fullFileName);

            userImg.SaveAs(pathToSave);
            customer.Image = "/Assets/images/users-photo/" + fullFileName;

            return customer;
        }

        public ActionResult DeleteProfile(int customerID)
        {
            Customer customer = customerRepository.Customers.FirstOrDefault(c => c.CustomerID == customerID);
            customerRepository.DeleteCustomer(customer);

            string path = Server.MapPath(customer.Image);
            FileInfo file = new FileInfo(path);
            if (file.Exists)
            {
                file.Delete();
            }

            return RedirectToAction("Account_Exit", "Authorization");
        }
    }
}