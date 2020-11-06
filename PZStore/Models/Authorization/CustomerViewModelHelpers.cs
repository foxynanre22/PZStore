using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PZStore.Models.Authorization
{
    public static class CustomerViewModelHelpers
    {
        public static RegisterModel ToViewModel(this Customer customer)
        {
            var registerModel = new RegisterModel
            {
                Email = customer.Email,
                Password = customer.Password,
                ConfirmPassword = customer.Password,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Country = customer.Country,
                Phone = customer.Phone
            };

            return registerModel;
        }

        public static Customer ToDomainModel(this RegisterModel registerModel)
        {
            var customer = new Customer();

            customer.Email = registerModel.Email;
            customer.Password = registerModel.Password;
            customer.FirstName = registerModel.FirstName;
            customer.LastName = registerModel.LastName;
            customer.Country = registerModel.Country;
            customer.Phone = registerModel.Phone;

            return customer;
        }
    }
}