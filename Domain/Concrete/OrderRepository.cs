using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class OrderRepository : IOrderRepository
    {
        PZStoreContext context = new PZStoreContext();
        public IEnumerable<Order> Orders
        {
            get { return context.Orders; }
        }
    }
}
