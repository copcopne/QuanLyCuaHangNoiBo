using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class EmployeeDAL
    {
        private readonly salesysdbEntities context;

        public EmployeeDAL(salesysdbEntities context)
        {
            this.context = context;
        }

        public List<Employee> GetEmployees(String keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return context.Employees.ToList();
            }
            else
            {
                return context.Employees
                    .Where(e => e.FullName.Contains(keyword))
                    .ToList();
            }
        }

        public Employee GetEmployeeById(int employeeId)
        {
            return context.Employees.FirstOrDefault(e => e.EmployeeID == employeeId);
        }

        public void AddEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("Đối tượng nhân viên là bắt buộc!");
            }
            context.Employees.Add(employee);
            context.SaveChanges();
        }

        public void UpdateEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException("Đối tượng nhân viên là bắt buộc!");
            }
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

        public void DeleteEmployee(int employeeId)
        {
            var employee = context.Employees.FirstOrDefault(e => e.EmployeeID == employeeId);
            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Nhân viên không tồn tại.");
            }
        }

    }
}
