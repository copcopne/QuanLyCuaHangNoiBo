using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class EmployeeDAL
    {
        private readonly salesysdbEntities context;

        public EmployeeDAL()
        {
            this.context = new salesysdbEntities();
        }

        public EmployeeDAL(salesysdbEntities context)
        {
            this.context = context;
        }

        public List<Employee> Get(String keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return context.Employees.ToList();
            }
            else
            {
                return context.Employees
                    .Where(e => e.FullName.Contains(keyword) || e.Email.StartsWith(keyword))
                    .ToList();
            }
        }

        public Employee Get(int employeeId)
        {
            return context.Employees.FirstOrDefault(e => e.EmployeeID == employeeId);
        }

        public void Add(Employee employee)
        {
            context.Employees.Add(employee);
            context.SaveChanges();
        }

        public void Update(Employee employee)
        {
            var existingEmployee = context.Employees.FirstOrDefault(e => e.EmployeeID == employee.EmployeeID);
            if (existingEmployee != null)
            {
                existingEmployee.FullName = employee.FullName;
                existingEmployee.Position = employee.Position;
                existingEmployee.Phone = employee.Phone;
                existingEmployee.Address = employee.Address;
                existingEmployee.Email = employee.Email;
                existingEmployee.Status = employee.Status;
                context.SaveChanges();
            }
        }

        public void Delete(int employeeId)
        {
            var employee = context.Employees.FirstOrDefault(e => e.EmployeeID == employeeId);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Nhân viên không tồn tại.");
            }
        }

    }
}
