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
        private InvoiceBUS invoiceBUS;
        private InvoiceDetailBUS invoiceDetailBUS;
        private ProductBUS productBUS;
        private Invoice invoice;

        public InvoiceDetailManagementForm(Invoice invoice)
        {
            InitializeComponent();
            this.invoice = invoice;
        }

        private void InvoiceDetailForm_Load(object sender, EventArgs e)
        {
            LoadInvoiceDetails(invoice.InvoiceID);
            SetHeader();
            labelTotalPrice.Text = $"Tổng tiền: {invoice.TotalAmount:N0} VND";
            labelTitle.Text += $"{invoice.InvoiceID}";

            dataGridView.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "btnEdit",
                HeaderText = "Hành động",
                Text = "Sửa",
                UseColumnTextForButtonValue = true,
                Width = 100
            });
            dataGridView.Columns.Add(new DataGridViewButtonColumn()
            {
                Name = "btnDelete",
                HeaderText = "Hành động",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 100
            });
            dataGridView.CellClick += DataGridView_CellClick;
        }

        private void SetHeader()
        {
            dataGridView.Columns["ProductName"].MinimumWidth = 200;
            dataGridView.Columns["ProductName"].HeaderText = "Tên sản phẩm";
            dataGridView.Columns["Quantity"].HeaderText = "Số lượng";
            dataGridView.Columns["UnitPrice"].HeaderText = "Đơn giá";
            dataGridView.Columns["TotalPrice"].HeaderText = "Thành tiền";
        }

        private void LoadInvoiceDetails(int invoiceId)
        {

            using (var context = new salesysdbEntities())
            {
                invoiceDetailBUS = new InvoiceDetailBUS(context);
                var invoiceDetails = invoiceDetailBUS.GetInvoiceDetails(invoiceId);
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
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            int productID = (int)dataGridView.Rows[e.RowIndex].Cells["ProductID"].Value;
            int quantity = (int)dataGridView.Rows[e.RowIndex].Cells["Quantity"].Value;

            if (dataGridView.Columns[e.ColumnIndex].Name == "btnEdit")
            {
                InvoiceDetailForm editForm = new InvoiceDetailForm(invoice, productID);
                editForm.ShowDialog();

                using (var context = new salesysdbEntities())
                {
                    invoiceBUS = new InvoiceBUS(context);
                    invoiceBUS.RecalculateTotalAmount(invoice.InvoiceID);
                }
                LoadInvoiceDetails(invoice.InvoiceID);
                ReloadTotalPrice();
            }
            else if (dataGridView.Columns[e.ColumnIndex].Name == "btnDelete")
            {
                if (dataGridView.Rows.Count <= 1)
                {
                    MessageBox.Show("Không thể xóa chi tiết cuối cùng của hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var confirmResult = MessageBox.Show("Bạn có chắc chắn muốn xóa sản phẩm này khỏi hóa đơn không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirmResult == DialogResult.Yes)
                {
                    using (var context = new salesysdbEntities())
                    {
                        invoiceDetailBUS = new InvoiceDetailBUS(context);
                        productBUS = new ProductBUS(context);
                        invoiceBUS = new InvoiceBUS(context);

                        invoiceDetailBUS.DeleteInvoiceDetail(invoice.InvoiceID, productID);

                        // Cập nhật số lượng tồn kho của sản phẩm
                        var product = context.Products.FirstOrDefault(p => p.ProductID == productID);
                        if (product != null)
                            productBUS.UpdateProductQuantity(productID, quantity);

                        invoiceBUS.RecalculateTotalAmount(invoice.InvoiceID);
                        context.SaveChanges();
                    }
                    ReloadTotalPrice();
                    MessageBox.Show("Xóa chi tiết hóa đơn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadInvoiceDetails(invoice.InvoiceID);
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            InvoiceDetailForm addForm = new InvoiceDetailForm(invoice);
            addForm.ShowDialog();

            using (var context = new salesysdbEntities())
            {
                invoiceBUS = new InvoiceBUS(context);
                invoiceBUS.RecalculateTotalAmount(invoice.InvoiceID);
            }
            ReloadTotalPrice();
            LoadInvoiceDetails(invoice.InvoiceID);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ReloadTotalPrice()
        {
            using (var context = new salesysdbEntities())
            {
                invoiceBUS = new InvoiceBUS(context);
                invoice = invoiceBUS.GetInvoiceById(invoice.InvoiceID);
                labelTotalPrice.Text = $"Tổng tiền: {invoice.TotalAmount:N0} VND";
            }
            
        }

    }

}

