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
        private CustomerBUS CustomerBUS;
        private Customer customer = new Customer();
        private readonly salesysdbEntities context = new salesysdbEntities();
        public CustomerForm()
        {
            InitializeComponent();
            CustomerBUS = new CustomerBUS(context);
        }
        public CustomerForm(Customer customer)
        {
            InitializeComponent();
            CustomerBUS = new CustomerBUS(context);
            txtFullName.Text = customer.FullName;
            txtEmail.Text = customer.Email;
            txtPhone.Text = customer.PhoneNumber;
            this.customer = customer;
            label.Text = "Sửa thông tin khách hàng";
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
            if (!ValidateInput(txtFullName, "Vui lòng nhập họ tên khách hàng.") ||
                !ValidateInput(txtPhone, "Vui lòng nhập số điện thoại khách hàng.") ||
                !ValidateInput(txtEmail, "Vui lòng nhập email khách hàng."))
            {
                return;
            }
            customer.FullName = txtFullName.Text.Trim();
            customer.Email = txtEmail.Text.Trim().ToLower();
            customer.PhoneNumber = txtPhone.Text.Trim();


            if (btnSave.Text == "Cập nhật")
            {
                CustomerBUS.UpdateCustomer(customer);
                MessageBox.Show("Cập nhật thông tin khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Tag = CustomerBUS.AddCustomer(customer);
                MessageBox.Show("Thêm khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Close();
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
            Close();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            // Nếu có dữ liệu được truyền vào, sửa tên tiêu đề của form
            if (!string.IsNullOrEmpty(txtFullName.Text) || !string.IsNullOrEmpty(txtEmail.Text) || !string.IsNullOrEmpty(txtPhone.Text))
            {
                Text = "Sửa thông tin khách hàng";
                btnSave.Text = "Cập nhật";
            }
            else
            {
                Text = "Thêm khách hàng mới";
                btnSave.Text = "Lưu";
            }
        }
    }
}
