using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PZStore.Models.Cart
{
    public class OrderDetails
    {
        [Required(ErrorMessage = "Enter your name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter your adress")]
        public string Adress { get; set; }

        [Required(ErrorMessage = "Enter your city")]
        public string City { get; set; }

        [Required(ErrorMessage = "Enter your country")]
        public string Country { get; set; }
    }
}