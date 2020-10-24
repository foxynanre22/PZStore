using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class CategoryRepository : ICategoryRepository
    {
        PZStoreContext context = new PZStoreContext();
        public IEnumerable<Category> Categories
        {
            get { return context.Categories; }
        }
    }
}
