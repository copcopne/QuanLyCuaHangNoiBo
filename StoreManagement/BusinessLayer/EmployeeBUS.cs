using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace BusinessLayer
{
    public class EmployeeBUS
    {
        private readonly DataAccessLayer.EmployeeDAL employeeDAL;

        public EmployeeBUS()
        {
            this.employeeDAL = new DataAccessLayer.EmployeeDAL();
        }

        public List<Entity.Employee> Get(String keyword)
        {
            return employeeDAL.Get(keyword);
        }

        public Entity.Employee GetByEmail(String email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new Exception("Email không được để trống.");
            }

            return employeeDAL.GetByEmail(email);
        }

        public Entity.Employee Get(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new Exception("Mã nhân viên không hợp lệ!");
            }
            return employeeDAL.Get(employeeId);
        }
        public void Add(Entity.Employee employee)
        {
            if (employee == null)
            {
                throw new Exception("Đối tượng nhân viên là bắt buộc!");
            }
            employeeDAL.Add(employee);
        }

        public void Update(Entity.Employee employee)
        {
            if (employee == null)
            {
                throw new Exception("Đối tượng nhân viên là bắt buộc!");
            }
            employeeDAL.Update(employee);
        }
        public void Delete(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new Exception("Mã nhân viên không hợp lệ!");
            }
            employeeDAL.Delete(employeeId);
        }
    }
}
