using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public static class AuthenticateBUS
    {
        private static Entity.UserAccount currentUser;
        private static readonly UserAccountBUS userAccountBUS = new UserAccountBUS();

        public static Entity.UserAccount CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        public static bool Authenticate(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Tên đăng nhập và mật khẩu là bắt buộc!");
            }
            var user = userAccountBUS.Authenticate(username, password);
            if (user != null)
            {
                currentUser = user;
                return true;
            }
            return false;

        }

        public static void Logout()
        {
            currentUser = null;
        }
    }
}
