using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class CustomerRepository : ICustomerRepository
    {
        PZStoreContext context = MainContext.context;
<<<<<<< HEAD
=======
>>>>>>> Admin
        public IEnumerable<Customer> Customers
        {
            get { return context.Customers; }
        }
    }
}
