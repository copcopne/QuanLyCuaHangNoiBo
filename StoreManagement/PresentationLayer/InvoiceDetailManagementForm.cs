using System;
using BusinessLayer;
using Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class InvoiceDetailManagementForm : Form
    {
        private readonly salesysdbEntities context = new salesysdbEntities();
        private readonly InvoiceDetailBUS invoiceDetailService;
        private Invoice invoice;
        public InvoiceDetailManagementForm(Invoice invoice)
        {
            InitializeComponent();
            this.invoiceDetailService = new InvoiceDetailBUS(context);
            this.invoice = invoice;
        }

        private void InvoiceDetailForm_Load(object sender, EventArgs e)
        {
            LoadInvoiceDetails(invoice.InvoiceID);
            setHeader();
            labelTotalPrice.Text = $"Tổng tiền: {invoice.TotalAmount.ToString("N0")} VND";
            labelTitle.Text = labelTitle.Text + $"{invoice.InvoiceID}";

            dataGridView.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "btnEdit",
                HeaderText = "Hành động",
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                Width = 100
            });
            dataGridView.CellClick += DataGridView_CellClick;
        }
        private void setHeader()
        {
            dataGridView.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dataGridView.Columns["Quantity"].HeaderText = "Số lượng";
            dataGridView.Columns["UnitPrice"].HeaderText = "Đơn giá";
            dataGridView.Columns["TotalPrice"].HeaderText = "Thành tiền";
        }
        private void LoadInvoiceDetails(int invoiceId)
        {
            List<InvoiceDetail> invoiceDetails = invoiceDetailService.GetInvoiceDetails(invoiceId);
            if (invoiceDetails == null || invoiceDetails.Count == 0)
            {
                MessageBox.Show("Hóa đơn này không có chi tiết sản phẩm nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dataGridView.DataSource = null;
                return;
            }
            var displayDetails = invoiceDetails.Select(id => new
            {
                id.ProductID,
                ProductName = id.Product != null ? id.Product.ProductName : "Không rõ",
                Quantity = id.Quantity,
                UnitPrice = id.UnitPrice,
                TotalPrice = id.Quantity * id.UnitPrice
            }).ToList();
            dataGridView.DataSource = displayDetails;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Font = new System.Drawing.Font("Arial", 12);
        }
        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dataGridView.Columns[e.ColumnIndex].Name == "btnEdit")
            {
                int productID = (int)dataGridView.Rows[e.RowIndex].Cells["ProductID"].Value;
                InvoiceDetailForm editForm = new InvoiceDetailForm(invoice, productID);
                editForm.ShowDialog();
                LoadInvoiceDetails(invoice.InvoiceID);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            InvoiceDetailForm addForm = new InvoiceDetailForm(invoice);
            addForm.ShowDialog();
            LoadInvoiceDetails(invoice.InvoiceID);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}

