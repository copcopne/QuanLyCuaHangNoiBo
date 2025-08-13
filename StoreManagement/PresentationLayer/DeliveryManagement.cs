using System;
using BusinessLayer;
using Entity;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class DeliveryManagement : Form
    {
        private DeliveryBUS deliveryBUS;
        private Timer debounceTimer;
        private String defaultSearchText = "Tìm kiếm theo địa chỉ giao hàng, nhân viên hoặc mã hóa đơn...";
        public DeliveryManagement()
        {
            InitializeComponent();
            debounceTimer = new Timer();
            debounceTimer.Interval = 300;
            debounceTimer.Tick += DebounceTimer_Tick;
        }
        private void DeliveryManagement_Load(object sender, EventArgs e)
        {
            LoadDeliveries();
            LoadDateTimePicker();
            LoadComboBox();
            cbStatus.SelectedIndexChanged += FilterChanged;
            dtpFromDate.ValueChanged += FilterChanged;
            dtpToDay.ValueChanged += FilterChanged;
            txtKeyword.Text = defaultSearchText;
            txtKeyword.ForeColor = Color.Gray;
            txtKeyword.Enter += txtKeyword_Enter;
            txtKeyword.Leave += txtKeyword_Leave;
            txtKeyword.TextChanged += txtKeyword_TextChanged;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadComboBox();
        }
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            LoadDeliveries();
        }
        private void LoadDateTimePicker()
        {
            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.CustomFormat = "'Từ ngày:' dd/MM/yyyy";
            dtpToDay.Format = DateTimePickerFormat.Custom;
            dtpToDay.CustomFormat = "'Đến ngày:' dd/MM/yyyy";

            dtpToDay.Value = DateTime.Today;
            dtpFromDate.Value = DateTime.Today.AddDays(-30);
        }
        private void LoadDeliveries()
        {
            var keyword = txtKeyword.Text.Trim();
            if (string.IsNullOrEmpty(keyword) || keyword == defaultSearchText)
                keyword = null;
            DateTime? fromDate = dtpFromDate.Value.Date;
            DateTime? toDate = dtpToDay.Value.Date;

            string status = cbStatus.SelectedItem?.ToString();

            using (salesysdbEntities context = new salesysdbEntities())
            {
                deliveryBUS = new DeliveryBUS(context);
                List<Delivery> deliveries = deliveryBUS.GetDeliveries(keyword, fromDate, toDate, status);
                dataGridView.DataSource = deliveries.Select(d => new
                {
                    d.DeliveryID,
                    d.InvoiceID,
                    Staff = d.Employee != null  ? d.Employee.FullName : "Chưa phân công",
                    DeliveryDate = d.DeliveryDate.HasValue ? d.DeliveryDate.Value.ToString("dd/MM/yyyy") : "Chưa giao",
                    d.DeliveryAddress,
                    d.Status
                }).ToList();
            }
        }
        private void LoadComboBox()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Tất cả");
            cbStatus.Items.Add("Chưa phân công");
            cbStatus.Items.Add("Chưa giao");
            cbStatus.Items.Add("Đang giao");
            cbStatus.Items.Add("Đã giao");
            cbStatus.Items.Add("Hủy");
            cbStatus.SelectedIndex = 0;
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
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                txtKeyword.Text = defaultSearchText;
                txtKeyword.ForeColor = Color.Gray;
            }
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }
        private void FilterChanged(object sender, EventArgs e)
        {
            LoadDeliveries();
        }
    }
}
