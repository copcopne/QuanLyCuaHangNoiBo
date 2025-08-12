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
            return customerDAL.GetCustomers(searchTerm);
        }
        public Customer AddCustomer(Customer customer)
        {
            return customerDAL.CreateCustomer(customer);
        }
        public Customer UpdateCustomer(Customer customer)
        {
            return customerDAL.UpdateCustomer(customer);
        }
        public void DeleteCustomer(int customerId)
        {
            customerDAL.DeleteCustomer(customerId);
        }
        public Customer GetCustomerByIdOrPhone(string searchTerm)
        {
            return customerDAL.GetCustomerByIdOrPhone(searchTerm);
        }
        public Customer GetCustomerById(int customerId)
        {
            if (customerId <= 0)
            {
                throw new ArgumentException("Mã khách hàng không được bé hơn 0", nameof(customerId));
            }
            return customerDAL.GetCustomerById(customerId);
        }
    }
}
