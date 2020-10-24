using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Entities
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        
        public string OrderAdress { get; set; }
        public string OrderEmail { get; set; }
        public DateTime OrderDate { get; set; }

        // Navigation properties
        public virtual ICollection<Product> Products { get; set; }
    }
}