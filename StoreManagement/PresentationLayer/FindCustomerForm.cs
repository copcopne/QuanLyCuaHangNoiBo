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
    public partial class FindCustomerForm : Form
    {
        private CustomerBUS customerBUS;
        private Timer debounceTimer;
        private String defaultSearchText = "Tìm kiếm theo tên, mã, số điện thoại hoặc email...";
        public FindCustomerForm()
        {
            InitializeComponent();
            debounceTimer = new Timer
            {
                Interval = 300
            };
            debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void FindCustomerForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
            this.ActiveControl = dataGridView;
            txtKeyword.Text = defaultSearchText;
            txtKeyword.ForeColor = SystemColors.GrayText;
            txtKeyword.Enter += txtKeyword_Enter;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.Font = new Font("Segoe UI", 12);
        }

        private void LoadCustomers()
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                customerBUS = new CustomerBUS(context);
                List<Customer> customers = customerBUS.GetCustomers(null);
                dataGridView.DataSource = customers.Select(c => new
                {
                    c.CustomerID,
                    c.FullName,
                    c.PhoneNumber,
                    c.Email
                }).ToList();
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Lấy khách hàng đã chọn
                int selectedCustomerId = (int)dataGridView.SelectedRows[0].Cells["CustomerID"].Value;
                using (salesysdbEntities context = new salesysdbEntities())
                {
                    customerBUS = new CustomerBUS(context);
                    Customer selectedCustomer = customerBUS.GetCustomerById(selectedCustomerId);
                    if (selectedCustomer != null)
                    {
                        DialogResult = DialogResult.OK;
                        Tag = selectedCustomer; // Trả về đối tượng khách hàng đã chọn
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy khách hàng đã chọn.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng.");
            }
        }

        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            string keyword = txtKeyword.Text.Trim();
            if (string.IsNullOrWhiteSpace(keyword) || keyword == defaultSearchText)
            {
                LoadCustomers();
            }
            else
            {
                using (salesysdbEntities context = new salesysdbEntities())
                {
                    customerBUS = new CustomerBUS(context);
                    List<Customer> customers = customerBUS.GetCustomers(keyword);
                    dataGridView.DataSource = customers.Select(c => new
                    {
                        c.CustomerID,
                        c.FullName,
                        c.PhoneNumber,
                        c.Email
                    }).ToList();
                }
            }

        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void txtKeyword_Enter(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
            if (txtKeyword.Text == defaultSearchText)
            {
                txtKeyword.Text = string.Empty;
                txtKeyword.ForeColor = SystemColors.WindowText;
            }

        }

        private void txtKeyword_Leave(object sender, EventArgs e)
        {
            dataGridView.ClearSelection();
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                txtKeyword.Text = defaultSearchText;
                txtKeyword.ForeColor = SystemColors.GrayText;
                LoadCustomers();
            }
            else
            {
                debounceTimer.Stop();
                debounceTimer.Start();
            }
        }
    }
}
