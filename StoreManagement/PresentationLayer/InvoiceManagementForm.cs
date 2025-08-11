using System;
using BusinessLayer;
using Entity;
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
    public partial class InvoiceManagementForm : Form
    {
        private readonly salesysdbEntities context = new salesysdbEntities();
        private readonly InvoiceBUS invoiceService;

        public InvoiceManagementForm()
        {
            InitializeComponent();
            this.invoiceService = new InvoiceBUS(context);
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void InvoiceManagement_Load(object sender, EventArgs e)
        {
            dataGridView.DataSource = invoiceService.GetInvoices().Select(i => new
            {
                i.InvoiceID,
                CustomerName = i.Customer != null ? i.Customer.FullName : "N/A",
                EmployeeName = i.Employee != null ? i.Employee.FullName : "N/A",
                i.TotalAmount,
                InvoiceDate = i.InvoiceDate.ToString("dd/MM/yyyy"),
                i.DeliveryRequired
            }).ToList();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            var employees = context.Employees.AsNoTracking().Select(emp => emp.FullName).ToList();
            employees.Insert(0, string.Empty);
            cbEmployee.DataSource = employees;

            dtpFromDate.Format = DateTimePickerFormat.Custom;
            dtpFromDate.CustomFormat = "'Từ ngày:' dd/MM/yyyy";
            dtpToDay.Format = DateTimePickerFormat.Custom;
            dtpToDay.CustomFormat = "'Đến ngày:' dd/MM/yyyy";

            var statuses = new List<string> { "Tất cả", "Cần giao hàng", "Không giao hàng" };
            var selectedStatus = cbStatus.SelectedItem as string;
            cbStatus.DataSource = statuses;

            if (selectedStatus != null)
                cbStatus.SelectedItem = selectedStatus;
            else
                cbStatus.SelectedIndex = 0; // Default to "Tất cả"

            dtpToDay.Value = DateTime.Today;
            dtpFromDate.Value = DateTime.Today.AddDays(-30);

        }

    }
}
