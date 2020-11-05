using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PZStore.Models.Account
{
    public class LoginModel
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class RegisterModel
    {
        [Required(ErrorMessage = "Please, enter valid email")]
        public string Email { get; set; }
        [Required]
        /*[StringLength(20, MinimumLength = 8, ErrorMessage = "Password lenght should be between 8 and 20 symbols")]*/
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords are different!")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Please, enter your first name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Please, enter your last name")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Please, choose country")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Please, enter your phone")]
        public string Phone { get; set; }
    }

    public enum Countries
    {
        USA,
        RUSSIA,
        CANADA,
        JAPAN,
        CHINA,
        FRANCE,
        GERMANY,
        POLAND,
        ENGLAND,
        AUSTRALIA,
        TURKEY,
        SPAIN
    }
}