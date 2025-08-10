using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserAccountDAL
    {
        private readonly EmployeeDAL employeeDAL;

        public UserAccountDAL()
        {
            this.employeeDAL = new EmployeeDAL();
        }

        public static Boolean Authenticate(String username, String password)
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                var user = context.UserAccounts.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    return user.PasswordHash == password;
                }
                return false;
            }
        }
        public List<UserAccount> Get(String keyword)
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                if (string.IsNullOrEmpty(keyword))
                {
                    return context.UserAccounts.ToList();
                }
                else
                {
                    return context.UserAccounts
                        .Where(e => e.Username.Contains(keyword))
                        .ToList();
                }
            }

        }
        public UserAccount GetByEmployeeID(int employeeID)
        {
            using (salesysdbEntities context = new salesysdbEntities())
                return context.UserAccounts.FirstOrDefault(u => u.EmployeeID == employeeID);
        }
        public UserAccount GetByUsername(String username)
        {
            using (salesysdbEntities context = new salesysdbEntities())
                return context.UserAccounts.FirstOrDefault(u => u.Username == username);
        }

        public void Add(UserAccount account, Employee employee)
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                if (account == null || employee == null)
                {
                    throw new Exception("Đối tượng nhân viên và người dùng là bắt buộc!");
                }

                employeeDAL.Add(employee);
                account.EmployeeID = employee.EmployeeID;
                context.UserAccounts.Add(account);
                context.SaveChanges();
            }

        }

        public void Update(UserAccount account)
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                if (account == null)
                {
                    throw new Exception("Đối tượng người dùng là bắt buộc!");
                }
                var existingUser = context.UserAccounts.FirstOrDefault(u => u.Username == account.Username);
                if (existingUser != null)
                {

                    existingUser.PasswordHash = account.PasswordHash;
                    existingUser.Role = account.Role;
                    existingUser.IsActive = account.IsActive;

                    context.SaveChanges();
                }
            }

        }

        public void Delete(int id)
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                var user = context.UserAccounts.FirstOrDefault(u => u.EmployeeID == id);
                if (user != null)
                {
                    context.UserAccounts.Remove(user);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Tài khoản không tồn tại.");
                }
            }

        }
    }
}
