using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PZStore.Models.ContactPage
{
    public class ContactPageModel
    {
        [Required(ErrorMessage = "Please, enter your name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please, enter your email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please, enter the message")]
        public string Message { get; set; }
    }
}