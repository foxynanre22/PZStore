using Domain.Abstract;
using Domain.Entities;
using PZStore.Models.Cart;
using PZStore.SyntaticSugar;
using PZStore.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PZStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        public CartController(IProductRepository repo)
        {
            repository = repo;
        }

        //Cart getting from session using Infrastructure.Binders.CartModelBinder (registered in Global.asax)
        public ActionResult Index(Cart cart, string returnUrl = "~/Home/Index")
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        //Cart getting from session using Infrastructure.Binders.CartModelBinder (registered in Global.asax)
        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        //Cart getting from session using Infrastructure.Binders.CartModelBinder (registered in Global.asax)
        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(g => g.ProductID == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Checkout(Cart cart)
        {
            return View(new OrderDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, OrderDetails orderDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, but your cart is empty!");
            }

            if (ModelState.IsValid)
            {
                string messageSubject = "Order to: " + orderDetails.Name + "@" + orderDetails.Email;

                string content = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Mail Templates/OrderMailTemplate.html"));

                StringBuilder orderHTML = new StringBuilder();

                foreach (var line in cart.Lines)
                {
                    var subtotal = line.Product.Price * line.Quantity;
                    orderHTML.AppendFormat("<h3>{0} x {1} || (total: {2:c})</h3>",
                        line.Product.Name, line.Quantity, subtotal);
                }
                orderHTML.AppendFormat("<h2>Total cost: {0:c}</h2>", cart.ComputeTotalValue());

                // look into file to understand where parameters are
                string message = string.Format(content, orderDetails.Email, orderDetails.Name, orderDetails.Adress,
                                                        orderDetails.City, orderDetails.Country, orderHTML);

                try
                {
                    EmailSender.SendHtmlEmailTo("pzstore9@gmail.com", messageSubject, message);
                    PZLogger.GetInstance().Info("CART_CONTROLLER::Order from " + orderDetails.Email + " has been accepted.");

                    cart.Clear();
                    return View("Completed");

                }
                catch (Exception e)
                {
                    PZLogger.GetInstance().Error("HOME_CONTROLLER::" + e.Message);
                    return View("~/Views/Shared/Error.cshtml");
                }
            }
            else
            {
                return View(orderDetails);
            }
        }
    }
}