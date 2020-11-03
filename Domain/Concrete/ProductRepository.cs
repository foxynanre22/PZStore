using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class ProductRepository : IProductRepository
    {
        PZStoreContext context = MainContext.context;
        public IEnumerable<Product> Products
        {
            get { return context.Products; }
        }
    }
}
