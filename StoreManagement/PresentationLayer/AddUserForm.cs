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
    public partial class AddUserForm : Form
    {
        private readonly EmployeeBUS employeeBUS = new EmployeeBUS();
        private readonly UserAccountBUS userAccountBUS = new UserAccountBUS();
        public AddUserForm()
        {
            InitializeComponent();
            cbEmployee.DisplayMember = "FullName";
            cbEmployee.ValueMember = "EmployeeID";

            var employees = employeeBUS.GetEmplyeesWithoutAccount() ?? new List<Employee>();
            employees.Insert(0, new Employee { EmployeeID = 0, FullName = "---Chọn nhân viên---" });

            cbEmployee.DataSource = employees;
            cbEmployee.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbEmployee.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng chọn nhân viên để thêm tài khoản.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtUsername.Text) || 
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                string.IsNullOrWhiteSpace(txtConfirm.Text) ||
                string.IsNullOrWhiteSpace(txtRole.Text))
            {
                MessageBox.Show("Thông tin không được để trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (txtPassword.Text != txtConfirm.Text)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (userAccountBUS.GetByUsername(txtUsername.Text) != null)
            {
                MessageBox.Show("Tên người dùng đã tồn tại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                UserAccount newAccount = new UserAccount
                {
                    Username = txtUsername.Text.Trim(),
                    PasswordHash = txtPassword.Text.Trim(),
                    Role = txtRole.Text.Trim(),
                    EmployeeID = (int)cbEmployee.SelectedValue
                };
                userAccountBUS.Add(newAccount);
                MessageBox.Show("Tài khoản đã được thêm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Đã xảy ra lỗi khi thêm tài khoản. Vui lòng thử lại sau.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddUserForm_Load(object sender, EventArgs e)
        {
        }
    }
}
