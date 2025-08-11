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
    public partial class UserForm : Form
    {
        private Entity.UserAccount userAccount;
        private readonly BusinessLayer.UserAccountBUS userAccountBUS;
        public UserForm(int employeeId)
        {
            this.userAccountBUS = new BusinessLayer.UserAccountBUS();

            InitializeComponent();

            if (employeeId > 0)
            {
                userAccount = userAccountBUS.GetByEmployeeID(employeeId);
                if (userAccount != null)
                {
                    this.txtUsername.Text = userAccount.Username;
                    this.txtRole.Text = userAccount.Role;
                    this.checkBoxIsActive.Checked = userAccount.IsActive;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            userAccount.Role = this.txtRole.Text;
            userAccount.IsActive = this.checkBoxIsActive.Checked;
            if (this.checkBoxChangePassword.Checked)
            {
                if (string.IsNullOrWhiteSpace(this.txtOldPassword.Text) || string.IsNullOrWhiteSpace(this.txtPassword.Text) || string.IsNullOrWhiteSpace(this.txtConfirm.Text))
                {
                    MessageBox.Show("Mật khẩu cũ, mới và xác nhận là bắt buộc.");
                    return;
                }
                if (this.txtPassword.Text != this.txtConfirm.Text)
                {
                    MessageBox.Show("Mật khẩu mới và xác nhận không khớp.");
                    return;
                }
                if (userAccountBUS.Authenticate(userAccount.Username, this.txtOldPassword.Text) != true)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng.");
                    return;
                }
                userAccount.PasswordHash = this.txtPassword.Text;
            }
            try
            {
                userAccountBUS.Update(userAccount);
                MessageBox.Show("Cập nhật tài khoản người dùng thành công.");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật tài khoản người dùng: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBoxChangePassword_CheckedChanged(object sender, EventArgs e)
        {
            this.groupPassword.Visible = this.checkBoxChangePassword.Checked;

            this.txtOldPassword.Clear();
            this.txtPassword.Clear();
            this.txtConfirm.Clear();
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            if (userAccount == null)
            {
                MessageBox.Show("Không tìm thấy tài khoản người dùng cho nhân viên này.");
                this.Close();

            }
        }
    }
}
