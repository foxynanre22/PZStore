using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZStore.Models.WorkWithCategories
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public String Description { get; set; }
        public string Image { get; set; }
        public string Stock { get; set; }

        public virtual ICollection<AssignedCategoryData> Categories { get; set; }
    }
}