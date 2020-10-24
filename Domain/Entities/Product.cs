using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Entities
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public String Description { get; set; }
        public string Image { get; set; }
        public string Stock { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}