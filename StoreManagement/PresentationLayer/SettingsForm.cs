using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) // Cập nhật mật khẩu
        {
            UserForm userForm = new UserForm(AuthenticateBUS.CurrentUser.EmployeeID);
            userForm.ShowDialog();

        }

        private void button2_Click(object sender, EventArgs e) // Đăng xuất
        {
            ((App)this.MdiParent)?.Logout();
        }
    }
}
