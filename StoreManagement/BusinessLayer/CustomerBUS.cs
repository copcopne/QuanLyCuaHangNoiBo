using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Entity;

namespace BusinessLayer
{
    public class CustomerBUS
    {
        private readonly CustomerDAL customerRepository;
        public CustomerBUS(salesysdbEntities context)
        {
            customerRepository = new CustomerDAL(context);
        }
        public List<Entity.Customer> GetCustomers(string searchTerm)
        {
            return customerRepository.getCustomers(searchTerm);
        }
        public Entity.Customer AddCustomer(in Entity.Customer customer)
        {
            return customerRepository.createCustomer(customer);
        }
        public Entity.Customer UpdateCustomer(Entity.Customer customer)
        {
            return customerRepository.updateCustomer(customer);
        }
        public void DeleteCustomer(int customerId)
        {
            customerRepository.deleteCustomer(customerId);
        }
    }
}
