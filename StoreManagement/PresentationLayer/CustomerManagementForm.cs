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
        private String defaultSearchText = "Tìm kiếm theo tên hoặc email...";
        public CustomerManagementForm()
        {
            InitializeComponent();

            // Khởi tạo Timer để debounce
            debounceTimer = new Timer();
            debounceTimer.Interval = 300;
            debounceTimer.Tick += DebounceTimer_Tick;
        }
        private void CustomerManagementForm_Load(object sender, EventArgs e)
        {
            dataGridView.DataSource = CustomerService.GetCustomers(null);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns["Invoices"].Visible = false;

            this.txtKeyword.Text = defaultSearchText;
            this.txtKeyword.ForeColor = Color.Gray;

            // Thêm cột nút sửa
            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "btnEdit";
            editButtonColumn.HeaderText = "Sửa";
            editButtonColumn.Text = "Sửa";
            editButtonColumn.UseColumnTextForButtonValue = true;
            dataGridView.Columns.Add(editButtonColumn);
            dataGridView.CellClick += DataGridView_CellClick;

        }
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView.Columns["btnEdit"].Index)
            {
                // Lấy thông tin khách hàng từ dòng đã chọn
                var selectedRow = dataGridView.Rows[e.RowIndex];
                var customer = (Entity.Customer)selectedRow.DataBoundItem;
                if (customer == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở form sửa thông tin khách hàng
                CustomerForm editCustomerForm = new CustomerForm(customer);
                editCustomerForm.ShowDialog();

                // Cập nhật lại danh sách khách hàng sau khi thoát form
                dataGridView.DataSource = CustomerService.GetCustomers(null);
            }
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            if (string.IsNullOrWhiteSpace(txtKeyword.Text) || txtKeyword.Text == defaultSearchText)
            {
                dataGridView.DataSource = CustomerService.GetCustomers(null);
                return;
            }
            dataGridView.DataSource = CustomerService.GetCustomers(txtKeyword.Text);
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            CustomerForm addCustomerForm = new CustomerForm();
            addCustomerForm.ShowDialog();

            // Cập nhật lại danh sách khách hàng sau khi thoát form
            dataGridView.DataSource = CustomerService.GetCustomers(null);

        }

        private void txtKeyword_Enter(object sender, EventArgs e)
        {
            if (txtKeyword.Text == defaultSearchText)
            {
                txtKeyword.Text = "";
                txtKeyword.ForeColor = Color.Black;
            }
        }

        private void txtKeyword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text) || txtKeyword.Text == defaultSearchText)
            {
                txtKeyword.Text = defaultSearchText;
                txtKeyword.ForeColor = Color.Gray;
                dataGridView.DataSource = CustomerService.GetCustomers(null);
            }
        }
    }
}
