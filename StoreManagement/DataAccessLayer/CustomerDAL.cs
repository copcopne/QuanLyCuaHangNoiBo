using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CustomerDAL
    {
        private readonly salesysdbEntities context = new salesysdbEntities();

        public List<Customer> getCustomers(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return context.Customers.ToList();
            }
            return context.Customers
                .Where(c => c.FullName.Contains(searchTerm) || c.Email.Contains(searchTerm))
                .ToList();
        }

        public Customer createCustomer(in Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
            return customer;
        }

        public Customer updateCustomer(Customer customer)
        {
            var existingCustomer = context.Customers.Find(customer.CustomerID);
            if (existingCustomer != null)
            {
                existingCustomer.FullName = customer.FullName;
                existingCustomer.PhoneNumber = customer.PhoneNumber;
                existingCustomer.Email = customer.Email;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Customer not found");
            }
            return existingCustomer;
        }

    }
}
