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

        static string GetHashedString(string input)
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

        static Boolean Authenticate(String username, String password)
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
        public List<UserAccount> GetUserAccounts(String keyword)
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

        public UserAccount GetUserAccountByUsername(String username)
        {
            return context.UserAccounts.FirstOrDefault(u => u.Username == username);
        }

        public void AddUserAccount(UserAccount account, Employee employee)
        {
            if (account == null)
            {
                throw new ArgumentNullException("Đối tượng nhân viên và người dùng là bắt buộc!");
            }

            employeeDAL.AddEmployee(employee);


            context.UserAccounts.Add(account);
            account.EmployeeID = employee.EmployeeID; // set ID của nhân viên cho tài khoản

            context.SaveChanges();
        }

        public void UpdateUserAccount(UserAccount account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("Đối tượng nhân viên là bắt buộc!");
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

        public void DeleteUserAccount(String username)
        {
            var user = context.UserAccounts.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                employeeDAL.DeleteEmployee(user.EmployeeID);
                context.UserAccounts.Remove(user);
                context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException("Tài khoản không tồn tại.");
            }
        }
    }
}
