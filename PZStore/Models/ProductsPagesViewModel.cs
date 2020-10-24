using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZStore.Models
{
    /*ViewModel for comfort manage data on ProductsRow partial view*/
    public class ProductsPagesViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}