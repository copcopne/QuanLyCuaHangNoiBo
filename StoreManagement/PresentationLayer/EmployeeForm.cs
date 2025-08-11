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
    public partial class EmployeeForm : Form
    {
        private readonly BusinessLayer.EmployeeBUS employeeBUS;
        private readonly BusinessLayer.UserAccountBUS userAccountBUS;
        private Entity.Employee employee;
        private Boolean isEditMode = false;
        public EmployeeForm(int employeeId)
        {
            employeeBUS = new BusinessLayer.EmployeeBUS();
            userAccountBUS = new BusinessLayer.UserAccountBUS();
            InitializeComponent();
            if (employeeId > 0)
            {
                isEditMode = true;

                this.checkHaveAccount.Visible = false;
                this.grpBoxAccount.Visible = false;
                employee = employeeBUS.Get(employeeId);
                if (employee != null)
                {
                    txtFullName.Text = employee.FullName;
                    txtEmail.Text = employee.Email;
                    txtAddress.Text = employee.Address;
                    txtPhone.Text = employee.Phone;
                    txtPosition.Text = employee.Position;
                    txtStatus.Text = employee.Status;
                }
                else
                {
                    MessageBox.Show("Nhân viên không tồn tại.");
                    this.Close();
                }
            }
            else
            {
                employee = new Entity.Employee();
                txtFullName.Clear();
                txtEmail.Clear();
            }
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {

        }

        private void checkHaveAccount_CheckedChanged(object sender, EventArgs e)
        {
            this.grpBoxAccount.Visible = this.checkHaveAccount.Checked;
            this.txtUsername.Clear();
            this.txtPassword.Clear();
            this.txtRole.Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Họ tên và email là bắt buộc.");
                return;
            }
            if (!isEditMode && employeeBUS.GetByEmail(txtEmail.Text.Trim()) != null ||
                isEditMode && (employee.Email.ToLower() != txtEmail.Text.ToLower().Trim() &&
                                employeeBUS.GetByEmail(txtEmail.Text.Trim()) != null))
            {
                MessageBox.Show("Email đã tồn tại.");
                return;
            }

            employee.FullName = txtFullName.Text.Trim();
            employee.Email = txtEmail.Text.Trim();
            employee.Address = txtAddress.Text.Trim();
            employee.Phone = txtPhone.Text.Trim();
            employee.Position = txtPosition.Text.Trim();
            employee.Status = txtStatus.Text.Trim();
            if (isEditMode)
            {
                try
                {
                    employeeBUS.Update(employee);
                    MessageBox.Show("Cập nhật nhân viên thành công.");
                }
                catch (Exception)
                {
                    MessageBox.Show($"Lỗi khi cập nhật nhân viên!");
                }

                this.Close();
            }
            else
            {
                if (checkHaveAccount.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtRole.Text))
                    {
                        MessageBox.Show("Tài khoản người dùng là bắt buộc.");
                        return;
                    }
                    Entity.UserAccount userAccount = new Entity.UserAccount
                    {
                        Username = txtUsername.Text.Trim(),
                        PasswordHash = txtPassword.Text.Trim(),
                        Role = txtRole.Text.ToUpper().Trim()
                    };
                    try
                    {
                        userAccountBUS.Add(userAccount, employee);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"Lỗi khi tạo tài khoản cho nhân viên!");
                        return;
                    }
                }
                else
                    try
                    {
                        employeeBUS.Add(employee);
                        MessageBox.Show("Thêm nhân viên thành công.");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show($"Lỗi khi thêm nhân viên!");
                        return;
                    }
                this.Close();
            }
        }

        private void btnCancel_CLick(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
