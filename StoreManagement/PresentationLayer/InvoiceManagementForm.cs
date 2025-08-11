using BusinessLayer;
using Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
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
                HeaderText = "Sửa",
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                Width = 100
            });

        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView.Columns["btnEdit"].Index)
            {
                var selectedRow = dataGridView.Rows[e.RowIndex];
                var invoiceId = (int)selectedRow.Cells["InvoiceID"].Value;
                var invoice = invoiceService.GetInvoiceById(invoiceId);

                var editForm = new InvoiceEditForm(invoice);
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
            DateTime? startDate = null;
            DateTime? endDate = null;
            var invoices = invoiceService.GetInvoicesFiltered(keyword, employeeName, deliveryRequired, startDate, endDate);
            dataGridView.DataSource = invoices.Select(i => new
            {
                i.InvoiceID,
                CustomerName = i.Customer.FullName,
                EmployeeName = i.Employee.FullName,
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
