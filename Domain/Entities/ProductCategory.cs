using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductCategory : IdentityUser
    {
        [Key]
        public int ID { get; set; }

        public int ProductID { get; set; }
        public int CategoryID { get; set; }
    }
}
