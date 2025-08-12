using DataAccessLayer;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserAccountBUS
    {
        private readonly DataAccessLayer.UserAccountDAL userDAL;

        public UserAccountBUS()
        {
            this.userDAL = new DataAccessLayer.UserAccountDAL();
        }

        public UserAccount Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Tên đăng nhập và mật khẩu là bắt buộc!");
            }
            return UserAccountDAL.Authenticate(username, Utils.GetHashedString(password));
        }
        public List<Entity.UserAccount> Get(string keyword)
        {
                return userDAL.Get(keyword);
        }

        public Entity.UserAccount GetByEmployeeID(int employeeId)
        {
            return userDAL.GetByEmployeeID(employeeId);
        }
        public Entity.UserAccount GetByUsername(string username)
        {
            return userDAL.GetByUsername(username);
        }

        public void Add(Entity.UserAccount account, Entity.Employee employee)
        {
            if (account == null || employee == null)
            {
                throw new Exception("Đối tượng tài khoản và nhân viên là bắt buộc!");
            }
            if (string.IsNullOrWhiteSpace(account.PasswordHash))
            {
                throw new Exception("Mật khẩu là bắt buộc!");
            }
            if (userDAL.GetByUsername(account.Username) != null)
            {
                throw new Exception("Tài khoản đã tồn tại.");
            }
            account.IsActive = true;
            account.PasswordHash = Utils.GetHashedString(account.PasswordHash);
            userDAL.Add(account, employee);
        }

        public void Add(Entity.UserAccount account)
        {
            if (account == null)
            {
                throw new Exception("Đối tượng tài khoản là bắt buộc!");
            }
            if (string.IsNullOrWhiteSpace(account.PasswordHash))
            {
                throw new Exception("Mật khẩu là bắt buộc!");
            }
            if (userDAL.GetByUsername(account.Username) != null)
            {
                throw new Exception("Tài khoản đã tồn tại.");
            }
            account.IsActive = true;
            account.PasswordHash = Utils.GetHashedString(account.PasswordHash);
            userDAL.Add(account);
        }

        public void Update(Entity.UserAccount account, Boolean isUpdatePassword = false)
        {
            if (account == null)
            {
                throw new Exception("Đối tượng tài khoản là bắt buộc!");
            }
            if(isUpdatePassword && string.IsNullOrWhiteSpace(account.PasswordHash))
            {
                throw new Exception("Mật khẩu là bắt buộc khi cập nhật.");
            }
            if (isUpdatePassword)
            {
                account.PasswordHash = Utils.GetHashedString(account.PasswordHash);
            }
            userDAL.Update(account);
        }
        public void Delete(int employeeId)
        {
            if (employeeId <= 0)
            {
                throw new Exception("ID nhân viên là bắt buộc!");
            }
            userDAL.Delete(employeeId);
        }
    }
}
