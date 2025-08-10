using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class UserAccountDAL
    {
        private readonly salesysdbEntities context;
        private readonly EmployeeDAL employeeDAL;

        public UserAccountDAL(salesysdbEntities context)
        {
            this.context = context;
            employeeDAL = new EmployeeDAL(context);
        }

        private static string GetHashedString(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                    sb.Append(b.ToString("x2"));

                return sb.ToString();
            }
        }

        public static Boolean Authenticate(String username, String password)
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                var user = context.UserAccounts.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    return user.PasswordHash == GetHashedString(password);
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

        public UserAccount GetByUserName(String username)
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
