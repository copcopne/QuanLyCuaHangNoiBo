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
    public partial class CustomerManagementForm : Form
    {
        private readonly salesysdbEntities context;
        private readonly CustomerBUS customerService;
        private Timer debounceTimer;
        private String defaultSearchText = "Tìm kiếm theo tên hoặc email...";
        public CustomerManagementForm()
        {
            InitializeComponent();
            this.context = new salesysdbEntities();
            this.customerService = new CustomerBUS(context);

            // Khởi tạo Timer để debounce
            debounceTimer = new Timer();
            debounceTimer.Interval = 300;
            debounceTimer.Tick += DebounceTimer_Tick;
        }
        private void CustomerManagementForm_Load(object sender, EventArgs e)
        {
            dataGridView.DataSource = customerService.GetCustomers(null);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Columns["Invoices"].Visible = false;
            dataGridView.Columns["IsDeleted"].Visible = false;

            txtKeyword.Text = defaultSearchText;
            txtKeyword.ForeColor = Color.Gray;

            
            // cho cột sửa và xóa nằm chung một cột
            dataGridView.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "btnEdit",
                HeaderText = "Sửa",
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                Width = 100
            });
            dataGridView.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "btnDelete",
                HeaderText = "Xóa",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 100
            });
            // Đăng ký sự kiện CellClick cho DataGridView
            dataGridView.CellClick += DataGridView_CellClick;
        }
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView.Columns["btnEdit"].Index)
            {
                // Lấy thông tin
                var selectedRow = dataGridView.Rows[e.RowIndex];
                var customer = (Entity.Customer)selectedRow.DataBoundItem;
                if (customer == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Mở form sửa
                CustomerForm editCustomerForm = new CustomerForm(customer);
                editCustomerForm.ShowDialog();

                // Cập nhật
                dataGridView.DataSource = customerService.GetCustomers(null);
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView.Columns["btnDelete"].Index)
            {
                // Lấy thông tin
                var selectedRow = dataGridView.Rows[e.RowIndex];
                var customer = (Entity.Customer)selectedRow.DataBoundItem;
                if (customer == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Xác nhận xóa
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa khách hàng {customer.FullName}?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    customerService.DeleteCustomer(customer.CustomerID);
                    dataGridView.DataSource = customerService.GetCustomers(null);
                }
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
                dataGridView.DataSource = customerService.GetCustomers(null);
                return;
            }
            dataGridView.DataSource = customerService.GetCustomers(txtKeyword.Text);
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            CustomerForm addCustomerForm = new CustomerForm();
            addCustomerForm.ShowDialog();

            // Cập nhật lại danh sách sau khi thoát form
            dataGridView.DataSource = customerService.GetCustomers(null);

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
                dataGridView.DataSource = customerService.GetCustomers(null);
            }
        }
    }
}
