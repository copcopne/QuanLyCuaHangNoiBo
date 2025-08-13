using BusinessLayer;
using Entity;
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
        private Entity.UserAccount currentUser => AuthenticateBUS.CurrentUser;
        private string role;
        private string targetRole;
        private bool isMySelf;
        private bool isAdmin;
        private bool isEmployeeManager;
        private bool canEditRole;
        private bool canEditStatus;
        private bool canChangePassword;
        private bool requireOldPassword;
        public UserForm(int employeeId)
        {
            this.userAccountBUS = new BusinessLayer.UserAccountBUS();
            role = currentUser?.Role.ToLower();
            isMySelf = currentUser != null && currentUser.EmployeeID == employeeId;
            isAdmin = role == "admin";
            isEmployeeManager = role == "employee_manager";

                InitializeComponent();

            if (currentUser != null && employeeId > 0)
            {
                userAccount = isMySelf ? currentUser : userAccountBUS.GetByEmployeeID(employeeId);
                if (userAccount != null)
                {
                    this.txtUsername.Text = userAccount.Username;
                    this.txtRole.Text = userAccount.Role;
                    this.checkBoxIsActive.Checked = userAccount.IsActive;

                    SetupAccessControls();
                }
            }
        }

        private void SetupAccessControls()
        {
            targetRole = userAccount.Role.ToLower();
            bool targetIsAdmin = targetRole == "admin";

            canEditRole = isAdmin;
            canEditStatus = isAdmin || (isEmployeeManager && !targetIsAdmin);

            if (isMySelf)
            {
                canChangePassword = true;
                requireOldPassword = true;
            }
            else
            {
                if (isAdmin)
                {
                    canChangePassword = true;
                    requireOldPassword = false;
                }
                else if (isEmployeeManager && !targetIsAdmin)
                {
                    canChangePassword = true;
                    requireOldPassword = false;
                }
                else
                {
                    canChangePassword = false;
                    requireOldPassword = true;
                }
            }

            this.txtRole.Enabled = canEditRole;
            this.checkBoxIsActive.Enabled = canEditStatus;

            this.txtOldPassword.Enabled = requireOldPassword;
            this.txtOldPassword.Visible = requireOldPassword;
            this.labelOld.Visible = requireOldPassword;
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            userAccount.Role = this.txtRole.Text;
            userAccount.IsActive = this.checkBoxIsActive.Checked;
            if (this.checkBoxChangePassword.Checked)
            {
                if (requireOldPassword && string.IsNullOrWhiteSpace(this.txtOldPassword.Text))
                {
                    MessageBox.Show("Mật khẩu cũ là bắt buộc.");
                    return;
                }
                if (string.IsNullOrWhiteSpace(this.txtPassword.Text) || string.IsNullOrWhiteSpace(this.txtConfirm.Text))
                {
                    MessageBox.Show("Mật khẩu mới và xác nhận là bắt buộc.");
                    return;
                }
                if (this.txtPassword.Text != this.txtConfirm.Text)
                {
                    MessageBox.Show("Mật khẩu mới và xác nhận không khớp.");
                    return;
                }
                if (requireOldPassword && userAccountBUS.Authenticate(userAccount.Username, this.txtOldPassword.Text) == null)
                {
                    MessageBox.Show("Mật khẩu cũ không đúng.");
                    return;
                }
                userAccount.PasswordHash = this.txtPassword.Text;
            }
            try
            {
                userAccountBUS.Update(userAccount, checkBoxChangePassword.Checked);
                MessageBox.Show("Cập nhật tài khoản người dùng thành công.");
                if(isMySelf)
                {
                    userAccount = userAccountBUS.GetByEmployeeID(currentUser.EmployeeID);
                    AuthenticateBUS.CurrentUser = userAccount;
                }

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
            if(!isMySelf && !isAdmin && !isEmployeeManager)
            {
                MessageBox.Show("Bạn không có quyền truy cập vào tài khoản người dùng này.");
                this.Close();
            }
        }
    }
}
