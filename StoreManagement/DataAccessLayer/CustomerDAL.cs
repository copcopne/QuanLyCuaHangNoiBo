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
        private readonly salesysdbEntities context;
        public CustomerDAL(salesysdbEntities context)
        {
            this.context = context;
        }

        public List<Customer> GetCustomers(string searchTerm)
        {
            // Lấy danh sách khách hàng từ cơ sở dữ liệu trừ các khách hàng đã bị xóa
            var query = context.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(c => c.FullName.Contains(searchTerm) ||
                                         c.PhoneNumber.Contains(searchTerm) ||
                                         c.Email.Contains(searchTerm) ||
                                         c.CustomerID.ToString() == searchTerm);
            }
            return query.Where(c => c.IsDeleted == 0).ToList();
        }

        public Customer CreateCustomer(in Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
            return customer;
        }

        public Customer UpdateCustomer(Customer customer)
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

        public void DeleteCustomer(int customerId)
        {
            var customer = context.Customers.Find(customerId);
            if (customer != null)
            {
                customer.IsDeleted = 1; // Đánh dấu là đã xóa
                customer.PhoneNumber = null;
                customer.Email = null;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Customer not found");
            }
        }

        public Customer GetCustomerByIdOrPhone(string identifier)
        {
            // Không trả id của khách hàng đã bị xóa
            if (int.TryParse(identifier, out int customerId))
                return context.Customers.FirstOrDefault(c => c.CustomerID == customerId && c.IsDeleted == 0);
            else
                return context.Customers.FirstOrDefault(c => c.PhoneNumber == identifier && c.IsDeleted == 0);
        }
        public Customer GetCustomerById(int customerId)
        {
            // Lấy khách hàng theo ID, không bao gồm khách hàng đã bị xóa
            return context.Customers.FirstOrDefault(c => c.CustomerID == customerId && c.IsDeleted == 0);
        }
    }
}
