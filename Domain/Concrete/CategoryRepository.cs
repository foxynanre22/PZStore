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
        PZStoreContext context = MainContext.context;
<<<<<<< HEAD
=======
>>>>>>> Admin
        public IEnumerable<Category> Categories
        {
            get { return context.Categories; }
        }

        public void SaveCategory(Category category)
        {
            if (category.CategoryID == 0)
            {
                context.Categories.Add(category);
            }
            else
            {
                Category dbCategory = context.Categories.Find(category.CategoryID);

                if (dbCategory != null)
                {
                    dbCategory.Name = category.Name;
                }
            }

            context.SaveChanges();
        }
    }
}
