using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZStore.Models
{
    /*Represent information about items on the current page and all pages*/
    public class PagingInfo
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages 
        { 
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); } 
        }
    }
}