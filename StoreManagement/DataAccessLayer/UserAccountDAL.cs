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
        private readonly salesysdbEntities context;
        private readonly EmployeeDAL employeeDAL;

        public UserAccountDAL()
        {
            this.context = new salesysdbEntities();
            this.employeeDAL = new EmployeeDAL(this.context);
        }
        public UserAccountDAL(salesysdbEntities context)
        {
            this.context = context;
            this.employeeDAL = new EmployeeDAL(context);
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

        public UserAccount GetByUsername(String username)
        {
            return context.UserAccounts.FirstOrDefault(u => u.Username == username);
        }

        public void Add(UserAccount account, Employee employee)
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

        public void Update(UserAccount account)
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

        public void Delete(String username)
        {
            var user = context.UserAccounts.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                employeeDAL.Delete(user.EmployeeID);
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
