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
        private Boolean isEditMode = false;
        private int employeeId = -1;
        public EmployeeForm(int employeeId)
        {
            employeeBUS = new BusinessLayer.EmployeeBUS();
            InitializeComponent();
            if(employeeId > 0)
            {
                this.employeeId = employeeId;
                isEditMode = true;

                this.checkHaveAccount.Visible = false;
                this.grpBoxAccount.Visible = false;
                var employee = employeeBUS.Get(employeeId);
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
                }
            }
            else
            {
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
            Entity.Employee employee = new Entity.Employee
            {
                FullName = txtFullName.Text,
                Email = txtEmail.Text,
                Address = txtAddress.Text,
                Phone = txtPhone.Text,
                Position = txtPosition.Text,
                Status = txtStatus.Text
            };
            if (isEditMode)
            {
                employee.EmployeeID = this.employeeId;
                employeeBUS.Update(employee);
                MessageBox.Show("Cập nhật nhân viên thành công.");

                btnCancel_CLick(sender, e);
            }
            else
            {
                if(checkHaveAccount.Checked)
                {
                    if (string.IsNullOrWhiteSpace(txtUsername.Text) || string.IsNullOrWhiteSpace(txtPassword.Text) || string.IsNullOrWhiteSpace(txtRole.Text))
                    {
                        MessageBox.Show("Tài khoản người dùng là bắt buộc.");
                        return;
                    }
                    Entity.UserAccount userAccount = new Entity.UserAccount
                    {
                        Username = txtUsername.Text,
                        PasswordHash = txtPassword.Text,
                        Role = txtRole.Text
                    };
                    userAccountBUS.Add(userAccount, employee);
                }
                employeeBUS.Add(employee);
                MessageBox.Show("Thêm nhân viên thành công.");

                btnCancel_CLick(sender, e);
            }
        }

        private void btnCancel_CLick(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
