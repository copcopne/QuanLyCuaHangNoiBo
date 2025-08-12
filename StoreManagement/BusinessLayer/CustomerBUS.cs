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
        private readonly CustomerDAL customerDAL;
        public CustomerBUS(salesysdbEntities context)
        {
            customerDAL = new CustomerDAL(context);
        }
        public List<Customer> GetCustomers(string searchTerm)
        {
            return customerDAL.getCustomers(searchTerm);
        }
        public Customer AddCustomer(Customer customer)
        {
            return customerDAL.createCustomer(customer);
        }
        public Customer UpdateCustomer(Customer customer)
        {
            return customerDAL.updateCustomer(customer);
        }
        public void DeleteCustomer(int customerId)
        {
            customerDAL.deleteCustomer(customerId);
        }
        public Customer GetCustomerByIdOrPhone(string searchTerm)
        {
            return customerDAL.GetCustomerByIdOrPhone(searchTerm);
        }
    }
}
