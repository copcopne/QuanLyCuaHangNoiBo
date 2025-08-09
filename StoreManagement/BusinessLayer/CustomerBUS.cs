using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class CustomerBUS
    {
        private readonly DataAccessLayer.CustomerDAL customerRepository = new DataAccessLayer.CustomerDAL();
        public List<Entity.Customer> GetAllCustomers()
        {
            return customerRepository.GetAllCustomers();
        }
        public List<Entity.Customer> GetCustomersByNameOrId(string searchTerm)
        {
            return customerRepository.getCustomersByNameOrId(searchTerm);
        }
        public Entity.Customer CreateCustomer(String fullName, String email, String phone)
        {
            String emailLower = email.ToLower();
            String fullNameTrimmed = fullName.Trim();
            Entity.Customer customer = new Entity.Customer
            {
                FullName = fullNameTrimmed,
                Email = emailLower,
                PhoneNumber = phone
            };
            return customerRepository.createCustomer(customer);
        }
        public Entity.Customer UpdateCustomer(Entity.Customer customer)
        {
            return customerRepository.updateCustomer(customer);
        }
    }
}
