using BusinessLayer;
using Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class InvoiceManagementForm : Form
    {
        private readonly salesysdbEntities context = new salesysdbEntities();
        private readonly InvoiceBUS invoiceService;
        private Timer debounceTimer;
        private String defaultSearchText = "Tìm kiếm theo tên khách hàng, nhân viên hoặc mã hóa đơn...";

        public InvoiceManagementForm()
        {
            InitializeComponent();
            this.invoiceService = new InvoiceBUS(context);

            debounceTimer = new Timer();
            debounceTimer.Interval = 300;
            debounceTimer.Tick += DebounceTimer_Tick;
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void InvoiceManagement_Load(object sender, EventArgs e)
        {
            LoadInvoices();
            setHeader();
            LoadComboBox();
            LoadDateTimePicker();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.CellFormatting += dataGridView_CellFormatting;

            txtKeyword.Text = defaultSearchText;
            txtKeyword.ForeColor = Color.Gray;

            cbEmployee.SelectedIndexChanged += FilterChanged;
            cbStatus.SelectedIndexChanged += FilterChanged;
            dtpFromDate.ValueChanged += FilterChanged;
            dtpToDay.ValueChanged += FilterChanged;

            dataGridView.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "btnEdit",
                HeaderText = "Hành động",
                Text = "Xem chi tiết",
                UseColumnTextForButtonValue = true,
                Width = 100
            });

        }
        private void setHeader()
        {
            dataGridView.Columns["InvoiceID"].HeaderText = "Mã hóa đơn";
            dataGridView.Columns["CustomerName"].HeaderText = "Tên khách hàng";
            dataGridView.Columns["EmployeeName"].HeaderText = "Tên nhân viên";
            dataGridView.Columns["TotalAmount"].HeaderText = "Tổng tiền";
            dataGridView.Columns["DeliveryRequired"].HeaderText = "Cần giao hàng";
            dataGridView.Columns["InvoiceDate"].HeaderText = "Ngày lập hóa đơn";

            dataGridView.Columns["InvoiceID"].Width = 100;
            dataGridView.Columns["CustomerName"].Width = 150;
            dataGridView.Columns["EmployeeName"].Width = 150;
            dataGridView.Columns["TotalAmount"].Width = 100;
            dataGridView.Columns["DeliveryRequired"].Width = 120;
            dataGridView.Columns["InvoiceDate"].Width = 120;
        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView.Columns["btnEdit"].Index)
            {
                var selectedRow = dataGridView.Rows[e.RowIndex];
                var invoiceId = (int)selectedRow.Cells["InvoiceID"].Value;
                var invoice = invoiceService.GetInvoiceById(invoiceId);

                var editForm = new InvoiceDetailManagementForm(invoice);
                editForm.ShowDialog();

                LoadInvoices();
            }
        }

        private void LoadInvoices()
        {
            var keyword = txtKeyword.Text.Trim();
            if (keyword == defaultSearchText || string.IsNullOrWhiteSpace(keyword))
            {
                keyword = null;
            }
            var employeeName = cbEmployee.SelectedItem as string;
            bool? deliveryRequired = null;
            if (cbStatus.SelectedItem != null)
            {
                if (cbStatus.SelectedItem.ToString() == "Cần giao hàng")
                {
                    deliveryRequired = true;
                }
                else if (cbStatus.SelectedItem.ToString() == "Không giao hàng")
                {
                    deliveryRequired = false;
                }
            }
            DateTime? startDate = dtpFromDate.Value;
            DateTime? endDate = dtpToDay.Value;
            var invoices = invoiceService.GetInvoicesFiltered(keyword, employeeName, deliveryRequired, startDate, endDate);
            dataGridView.DataSource = invoices.Select(i => new
            {
                i.InvoiceID,
                CustomerName =  i.Customer.FullName != null ? i.Customer.FullName : "Khách",
                EmployeeName = i.Employee.FullName != null ? i.Employee.FullName : "Không rõ",
                TotalAmount = i.TotalAmount,
                i.DeliveryRequired,
                InvoiceDate = i.InvoiceDate,
            }).ToList();

        }
        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView.Columns[e.ColumnIndex].Name == "InvoiceDate")
            {
                if (e.Value != null && e.Value is DateTime dateTime)
                {
                    e.Value = dateTime.ToString("dd/MM/yyyy");
                    e.FormattingApplied = true;
                }
            }
        }

        private void LoadComboBox()
        {
            var employees = context.Employees.AsNoTracking().Select(emp => emp.FullName).ToList();
            employees.Insert(0, string.Empty);
            cbEmployee.DataSource = employees;

            var statuses = new List<string> { "Tất cả", "Cần giao hàng", "Không giao hàng" };
            var selectedStatus = cbStatus.SelectedItem as string;
            cbStatus.DataSource = statuses;

            if (selectedStatus != null)
                cbStatus.SelectedItem = selectedStatus;
            else
                cbStatus.SelectedIndex = 0;

        }
        private void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            LoadInvoices();
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


        private void txtKeyword_Enter(object sender, EventArgs e)
        {
            if (txtKeyword.Text == defaultSearchText)
            {
                txtKeyword.Text = string.Empty;
                txtKeyword.ForeColor = Color.Black;
            }
        }

        private void txtKeyword_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtKeyword.Text))
            {
                txtKeyword.Text = defaultSearchText;
                txtKeyword.ForeColor = Color.Gray;
                LoadInvoices();
            }
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            LoadInvoices();
        }
    }
}
