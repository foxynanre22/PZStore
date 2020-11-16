using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class RoleRepository : IRoleRepository, IDisposable
    {
        PZStoreContext context = MainContext.context;

        public IEnumerable<Role> Roles 
        {
            get { return context.Roles; }
        }


        //for using class like resource (in using() block)
        public void Dispose()
        {
        }
    }
}
