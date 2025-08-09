using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CustomerRepository
    {
        private readonly salesysdbEntities context;

        public CustomerRepository(salesysdbEntities context)
        {
            this.context = context;
        }

        public List<Customer> GetAllCustomers()
        {
            return context.Customers.ToList();
        }
    }
}
