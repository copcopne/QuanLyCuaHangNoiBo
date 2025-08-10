using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class UserAccountBUS
    {
        private readonly DataAccessLayer.UserAccountDAL userDAL = new DataAccessLayer.UserAccountDAL();
        public List<Entity.UserAccount> Get(string keyword)
        {
                return userDAL.Get(keyword);
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

            account.PasswordHash = Utils.GetHashedString(account.PasswordHash);
            userDAL.Add(account, employee);
        }

        public void Update(Entity.UserAccount account)
        {
            if (account == null)
            {
                throw new Exception("Đối tượng tài khoản là bắt buộc!");
            }
            userDAL.Update(account);
        }
    }
}
