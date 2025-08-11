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
    public partial class InvoiceDetailForm : Form
    {
        private readonly salesysdbEntities context = new salesysdbEntities();
        private readonly InvoiceDetailBUS invoiceDetailBUS;
        private readonly CategoryBUS categoryBUS;
        private readonly ProductBUS productBUS;
        private readonly Invoice invoice;
        private InvoiceDetail invoiceDetail;

        public InvoiceDetailForm(Invoice invoice, int productID)
        {
            InitializeComponent();
            this.invoice = invoice;
            this.invoiceDetailBUS = new InvoiceDetailBUS(context);
            this.categoryBUS = new CategoryBUS(context);
            this.productBUS = new ProductBUS(context);
            this.invoiceDetail = invoice.InvoiceDetails.FirstOrDefault(id => id.ProductID == productID);
        }
        public InvoiceDetailForm(Invoice invoice)
        {
            InitializeComponent();
            this.invoice = invoice;
            this.invoiceDetailBUS = new InvoiceDetailBUS(context);
            this.categoryBUS = new CategoryBUS(context);
            this.productBUS = new ProductBUS(context);
        }

        private void InvoiceDetailForm_Load(object sender, EventArgs e)
        {
            LoadComboBox();
            cbCategory.SelectedIndexChanged += cbCategory_SelectedIndexChanged;
            if(this.invoiceDetail != null)
            {
                LoadInvoiceDetail(invoiceDetail);
                cbCategory.Enabled = false;
                cbProductName.Enabled = false;
            }

        }

        private void LoadInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            cbProductName.SelectedValue = invoiceDetail.ProductID;
            txtAmount.Text = invoiceDetail.Quantity.ToString();
            txtUnitPrice.Text = invoiceDetail.UnitPrice.ToString();
        }

        private void LoadComboBox()
        {
            var categories = categoryBUS.GetCategories();
            categories.Insert(0, new Category { CategoryID = 0, CategoryName = "-- Tất cả --" });
            cbCategory.DataSource = categories;
            cbCategory.DisplayMember = "CategoryName";
            cbCategory.SelectedIndex = 0;
            cbProductName.DataSource = productBUS.GetProducts(null);
            cbProductName.DisplayMember = "ProductName";
            cbProductName.ValueMember = "ProductID";
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCategory = cbCategory.SelectedItem as Category;
            if (selectedCategory.CategoryID != 0)
            {
                int selectedCategoryId = selectedCategory.CategoryID;
                var productsInCategory = productBUS.GetProductsByCategory(selectedCategoryId);
                cbProductName.DataSource = productsInCategory;
                return;
            }
            cbProductName.DataSource = productBUS.GetProducts(null);
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // chỉ cho số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cbProductName.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtAmount.Text) || string.IsNullOrWhiteSpace(txtUnitPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập số lượng và đơn giá.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (invoiceDetail == null)
            {
                var existingDetail = invoiceDetailBUS.GetInvoiceDetails(invoice.InvoiceID)
                    .FirstOrDefault(id => id.ProductID == (int)cbProductName.SelectedValue);
                if (existingDetail != null)
                {
                    MessageBox.Show("Chi tiết hóa đơn cho sản phẩm này đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                invoiceDetail = new InvoiceDetail
                {
                    InvoiceID = invoice.InvoiceID,
                    ProductID = (int)cbProductName.SelectedValue,
                    Quantity = int.Parse(txtAmount.Text),
                    UnitPrice = long.Parse(txtUnitPrice.Text),
                };
                invoiceDetailBUS.AddInvoiceDetail(invoiceDetail);
                MessageBox.Show("Thêm chi tiết hóa đơn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                invoiceDetail.Quantity = int.Parse(txtAmount.Text);
                invoiceDetail.UnitPrice = long.Parse(txtUnitPrice.Text);
                invoiceDetailBUS.UpdateInvoiceDetail(invoiceDetail);
                MessageBox.Show("Cập nhật chi tiết hóa đơn thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.Close();
        }
    }
}
