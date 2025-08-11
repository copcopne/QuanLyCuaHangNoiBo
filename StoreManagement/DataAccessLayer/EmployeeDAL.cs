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
        public List<Employee> Get(String keyword)
        {
            using (salesysdbEntities context = new salesysdbEntities())
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
        }

        public Employee Get(int employeeID)
        {
            using (salesysdbEntities context = new salesysdbEntities())
                return context.Employees.FirstOrDefault(e => e.EmployeeID == employeeID);
        }
        public Employee GetByEmail(String email)
        {
            using (salesysdbEntities context = new salesysdbEntities())
                return context.Employees.FirstOrDefault(e => e.Email == email);
        }

        public void Add(Employee employee)
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                context.Employees.Add(employee);
                context.SaveChanges();
            }
        }

        public void Update(Employee employee)
        {
            using (salesysdbEntities context = new salesysdbEntities())
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
        }

        public void Delete(int employeeID)
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                var employee = context.Employees.FirstOrDefault(e => e.EmployeeID == employeeID);
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
}
