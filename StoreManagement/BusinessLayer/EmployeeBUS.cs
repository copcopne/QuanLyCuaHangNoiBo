using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class EmployeeBUS
    {
        private readonly DataAccessLayer.EmployeeDAL employeeDAL = new DataAccessLayer.EmployeeDAL();

        public List<Entity.Employee> Get(String keyword)
        {
            return employeeDAL.Get(keyword);
        }

        public Entity.Employee Get(int employeeId)
        {
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
    }
}
