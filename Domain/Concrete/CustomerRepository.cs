using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class CustomerRepository : ICustomerRepository, IDisposable
    {
        PZStoreContext context = MainContext.context;

        public IEnumerable<Customer> Customers
        {
            get { return context.Customers; }
        }

        public void SaveCustomer(Customer customer)
        {
            if (customer.CustomerID == 0)
            {
                customer.RoleID = 2;
                context.Customers.Add(customer);
            }
            else
            {
                Customer dbCustomer = context.Customers.Find(customer.CustomerID);

                if (dbCustomer != null)
                {
                    dbCustomer.Email = customer.Email;
                    dbCustomer.ConfirmedEmail = customer.ConfirmedEmail;
                    dbCustomer.Password = customer.Password;
                    dbCustomer.FirstName = customer.FirstName;
                    dbCustomer.LastName = customer.LastName;
                    dbCustomer.Country = customer.Country;
                    dbCustomer.Phone = customer.Phone;
                    dbCustomer.Orders = customer.Orders;
                    dbCustomer.RoleID = 2;
                }
            }

            context.SaveChanges();
        }

        public void DeleteCustomer(Customer customer)
        {
            if (customer.CustomerID != 0)
            {
                Customer dbCustomer = context.Customers.Find(customer.CustomerID);

                if (dbCustomer != null)
                {
                    context.Customers.Remove(dbCustomer);
                    context.SaveChanges();
                }
            }
        }

        //for using class like resource (in using() block)
        public void Dispose()
        {
        }
    }
}
