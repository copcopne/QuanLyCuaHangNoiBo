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
        private Timer debounceTimer;
        private String defaultSearchText = "Tìm kiếm theo tên khách hàng, nhân viên hoặc mã hóa đơn...";
        private InvoiceBUS invoiceBUS;
        public InvoiceManagementForm()
        {
            InitializeComponent();

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
            LoadComboBox();
            LoadDateTimePicker();
            LoadInvoices();
            SetHeader();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txtKeyword.Text = defaultSearchText;
            txtKeyword.ForeColor = Color.Gray;

            cbEmployee.SelectedIndexChanged += FilterChanged;
            cbStatus.SelectedIndexChanged += FilterChanged;
            dtpFromDate.ValueChanged += FilterChanged;
            dtpToDay.ValueChanged += FilterChanged;

            dataGridView.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "btnDetails",
                HeaderText = "",
                Text = "Xem chi tiết",
                UseColumnTextForButtonValue = true,
                Width = 100
            });
            // Nút xóa
            dataGridView.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                Name = "btnDelete",
                HeaderText = "",
                Width = 120,
                TrueValue = true,
                FalseValue = false
            });

        }
        private void SetHeader()
        {
            dataGridView.Columns["InvoiceID"].HeaderText = "Mã hóa đơn";
            dataGridView.Columns["CustomerName"].HeaderText = "Tên khách hàng";
            dataGridView.Columns["EmployeeName"].HeaderText = "Tên nhân viên";
            dataGridView.Columns["TotalAmount"].HeaderText = "Tổng tiền";
            dataGridView.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
            dataGridView.Columns["DeliveryRequired"].HeaderText = "Cần giao hàng";
            dataGridView.Columns["InvoiceDate"].HeaderText = "Ngày lập hóa đơn";
            dataGridView.Columns["InvoiceDate"].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView.Columns["btnDetails"].Index)
            {
                var selectedRow = dataGridView.Rows[e.RowIndex];
                var invoiceId = (int)selectedRow.Cells["InvoiceID"].Value;
                using(var context = new salesysdbEntities())
                {
                    invoiceBUS = new InvoiceBUS(context);
                    Invoice invoice = invoiceBUS.GetInvoiceById(invoiceId);
                    if (invoice == null)
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    var editForm = new InvoiceDetailManagementForm(invoice);
                    editForm.ShowDialog();
                }
                LoadInvoices();
            }

            if (e.RowIndex >= 0 && e.ColumnIndex == dataGridView.Columns["DeliveryRequired"].Index)
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn thay đổi trạng thái giao hàng không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;

                var selectedRow = dataGridView.Rows[e.RowIndex];
                int invoiceId = (int)selectedRow.Cells["InvoiceID"].Value;

                using (var context = new salesysdbEntities())
                {
                    invoiceBUS = new InvoiceBUS(context);
                    Invoice invoice = invoiceBUS.GetInvoiceById(invoiceId);

                    if (invoice == null)
                    {
                        MessageBox.Show("Không tìm thấy hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // BẬT hay TẮT yêu cầu giao
                    bool willEnableDelivery = !invoice.DeliveryRequired;

                    // Nếu user muốn TẮT yêu cầu giao (true -> false) thì kiểm tra xem đơn đã giao chưa
                    if (!willEnableDelivery && invoice.DeliveryRequired)
                    {
                        bool alreadyDelivered = false;

                        if (context.Deliveries.Any(d => d.InvoiceID == invoice.InvoiceID && d.Status == "Đã giao"))
                            alreadyDelivered = true;

                        if (alreadyDelivered)
                        {
                            MessageBox.Show("Không thể tắt yêu cầu giao: đơn này đã được giao.", "Không cho phép", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Nếu người dùng định bật Delivery nhưng invoice chưa có customer thì yêu cầu chọn/tạo customer trước
                    if (willEnableDelivery && invoice.Customer == null)
                    {
                        var action = MessageBox.Show(
                            "Hóa đơn này chưa có thông tin khách hàng. Bạn muốn Chọn khách hàng có sẵn?\nChọn No để tạo khách hàng mới.",
                            "Chọn hay tạo mới",
                            MessageBoxButtons.YesNoCancel,
                            MessageBoxIcon.Question
                        );

                        if (action == DialogResult.Yes)
                        {
                            using (var findCustomerForm = new FindCustomerForm())
                            {
                                if (findCustomerForm.ShowDialog() == DialogResult.OK)
                                {
                                    Customer selectedCustomer = findCustomerForm.Tag as Customer;
                                    if (selectedCustomer != null)
                                        invoice.CustomerID = selectedCustomer.CustomerID;
                                    else
                                    {
                                        MessageBox.Show("Không tìm thấy khách hàng đã chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                                else
                                    return; // user hủy tìm kiếm -> không thay đổi
                            }
                        }
                        else if (action == DialogResult.No)
                        {
                            using (var createCustomerForm = new CustomerForm())
                            {
                                if (createCustomerForm.ShowDialog() == DialogResult.OK)
                                {
                                    if (createCustomerForm.Tag is Customer newCustomer)
                                        invoice.CustomerID = newCustomer.CustomerID;
                                    else
                                    {
                                        MessageBox.Show("Không thể tạo khách hàng mới.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                                else
                                {
                                    return; // user hủy tạo -> không thay đổi
                                }
                            }
                        }
                        else // Cancel
                        {
                            MessageBox.Show("Vui lòng chọn hoặc tạo khách hàng trước khi bật giao hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }

                    // Tại đây: hoặc invoice đã có customer, hoặc user vừa chọn/tạo customer, hoặc willEnableDelivery == false
                    bool previousState = invoice.DeliveryRequired;
                    invoice.DeliveryRequired = willEnableDelivery;

                    invoiceBUS.UpdateInvoice(invoice);

                    // Nếu vừa bật yêu cầu giao thì tạo Delivery; nếu tắt thì có thể huỷ delivery (tuỳ nghiệp vụ)
                    if (willEnableDelivery && !previousState)
                    {
                        CreateDelivery(invoice.InvoiceID);
                    }
                    else if (!willEnableDelivery && previousState)
                    {
                        DeliveryBUS deliveryBUS = new DeliveryBUS(context);
                        deliveryBUS.CancelDelivery(invoice);
                    }
                }

                LoadInvoices(); // load lại sau khi xử lý
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
            using(salesysdbEntities context = new salesysdbEntities())
            {
                InvoiceBUS invoiceBUS = new InvoiceBUS(context);
                var invoices = invoiceBUS.GetInvoicesFiltered(keyword, employeeName, deliveryRequired, startDate, endDate);
                dataGridView.DataSource = invoices.Select(i => new
                {
                    i.InvoiceID,
                    CustomerName = i.Customer?.FullName ?? "Khách",
                    EmployeeName = i.Employee?.FullName ?? "Không rõ",
                    i.TotalAmount,
                    i.DeliveryRequired,
                    i.InvoiceDate,
                }).ToList();
            }

        }

        private void LoadComboBox()
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                InvoiceBUS invoiceBUS = new InvoiceBUS(context);
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

        private void CreateDelivery(int invoiceId)
        {
            // Mở form tạo giao hàng
            using (salesysdbEntities context = new salesysdbEntities())
            {
                var deliveryForm = new DeliveryForm(invoiceId);
                if (deliveryForm.ShowDialog() == DialogResult.OK)
                {
                    LoadInvoices();
                }
            }
        }
    }
}
