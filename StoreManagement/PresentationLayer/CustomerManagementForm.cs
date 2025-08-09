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
    public partial class CustomerManagementForm : Form
    {
        private CustomerBUS CustomerService = new CustomerBUS();
        private Timer debounceTimer;
        public CustomerManagementForm()
        {
            InitializeComponent();

            // Khởi tạo Timer để debounce
            debounceTimer = new Timer();
            debounceTimer.Interval = 500;
            debounceTimer.Tick += DebounceTimer_Tick;
        }
        private void CustomerManagementForm_Load(object sender, EventArgs e)
        {
            dataGridView.DataSource = CustomerService.GetAllCustomers();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns["Invoices"].Visible = false;
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            dataGridView.DataSource = CustomerService.GetCustomersByNameOrId(txtKeyword.Text);
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            dataGridView.DataSource = CustomerService.CreateCustomer(txtFullName.Text, txtEmail.Text, txtPhone.Text);
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // chặn ký tự không phải số
            }
        }
    }
}
