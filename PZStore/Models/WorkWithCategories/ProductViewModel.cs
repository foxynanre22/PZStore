using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PZStore.Models.WorkWithCategories
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Please, enter name of the product")]
        public string Name { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price should be a positive number")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Please, enter description of the product")]
        public String Description { get; set; }

        [Required(ErrorMessage = "Please, enter the path to the product image")]
        public string Image { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Stock should be a positive number")]
        public int Stock { get; set; }

        public virtual ICollection<AssignedCategoryData> Categories { get; set; }
    }
}