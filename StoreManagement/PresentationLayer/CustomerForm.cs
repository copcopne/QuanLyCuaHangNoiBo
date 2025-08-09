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
    public partial class CustomerForm : Form
    {
        private CustomerBUS CustomerService = new CustomerBUS();
        private Customer customer = new Customer();
        public CustomerForm()
        {
            InitializeComponent();
        }
        public CustomerForm(Customer customer)
        {
            InitializeComponent();
            this.txtFullName.Text = customer.FullName;
            this.txtEmail.Text = customer.Email;
            this.txtPhone.Text = customer.PhoneNumber;
            this.customer = customer;
        }

        private bool ValidateInput(TextBox txt, string message)
        {
            if (string.IsNullOrWhiteSpace(txt.Text))
            {
                MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt.Focus();
                return false;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Kiểm tra các trường nhập liệu
            if (!ValidateInput(this.txtFullName, "Vui lòng nhập họ tên khách hàng.") ||
                !ValidateInput(this.txtPhone, "Vui lòng nhập số điện thoại khách hàng.") ||
                !ValidateInput(this.txtEmail, "Vui lòng nhập email khách hàng."))
            {
                return;
            }
            customer.FullName = this.txtFullName.Text.Trim();
            customer.Email = this.txtEmail.Text.Trim().ToLower();
            customer.PhoneNumber = this.txtPhone.Text.Trim();


            if (this.btnSave.Text == "Cập nhật")
            {
                CustomerService.UpdateCustomer(customer);
                MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                CustomerService.AddCustomer(customer);
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Ngăn chặn ký tự không phải số
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            // Nếu có dữ liệu được truyền vào, sửa tên tiêu đề của form
            if (!string.IsNullOrEmpty(txtFullName.Text) || !string.IsNullOrEmpty(txtEmail.Text) || !string.IsNullOrEmpty(txtPhone.Text))
            {
                this.Text = "Sửa thông tin khách hàng";
                this.btnSave.Text = "Cập nhật";
            }
            else
            {
                this.Text = "Thêm khách hàng mới";
                this.btnSave.Text = "Lưu";
            }
        }
    }
}
