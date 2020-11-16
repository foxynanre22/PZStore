using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace PZStore.Providers
{
    public class CustomeRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            string[] user_roles = new string[] { };
            
            using (CustomerRepository repo = new CustomerRepository())
            {
                Customer customer = repo.Customers.FirstOrDefault(c => c.Email == username);
                if (customer != null)
                {
                    using(RoleRepository roleRepo = new RoleRepository())
                    {
                        Role customerRole = roleRepo.Roles.Where(r => r.RoleID == customer.RoleID).FirstOrDefault();
                        if(customerRole != null)
                        {
                            user_roles = new string[] { customerRole.Name };
                        }
                    }  
                }
            }

            return user_roles;
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            bool result = false;

            using (CustomerRepository repo = new CustomerRepository())
            {
                Customer customer = repo.Customers.FirstOrDefault(c => c.Email == username);
                if (customer != null)
                {
                    using (RoleRepository roleRepo = new RoleRepository())
                    {
                        Role customerRole = roleRepo.Roles.Where(r => r.RoleID == customer.RoleID).FirstOrDefault();
                        if (customerRole != null && customerRole.Name == roleName)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}