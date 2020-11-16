using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Entities
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }

        public string Email { get; set; }
        public bool ConfirmedEmail { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; }

        public Customer()
        {
            Orders = new List<Order>();
        }
    }

    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        public string Name { get; set; }
    }
}