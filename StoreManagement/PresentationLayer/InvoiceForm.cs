using System;
using BusinessLayer;
using Entity;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Printing;

namespace PresentationLayer
{
    public partial class InvoiceForm : Form
    {
        private ProductBUS productBUS;
        private InvoiceBUS invoiceBUS;
        private CategoryBUS categoryBUS;

        public InvoiceForm()
        {
            InitializeComponent();
        }


        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            LoadComboBox();
            cbCategory.SelectedIndexChanged += cbCategory_SelectedIndexChanged;
        }

        private void LoadComboBox()
        {
            using (var context = new salesysdbEntities())
            {
                productBUS = new ProductBUS(context);
                var categories = categoryBUS.GetCategories();
                categories.Insert(0, new Category { CategoryID = 0, CategoryName = "-- Tất cả --" });
                cbCategory.DataSource = categories;
                cbCategory.DisplayMember = "CategoryName";
                cbCategory.SelectedIndex = 0;
                cbProductName.DataSource = productBUS.GetActiveProducts();
                cbProductName.DisplayMember = "ProductName";
                cbProductName.ValueMember = "ProductID";
            }
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
            cbProductName.DataSource = productBUS.GetActiveProducts();
        }

        private void txtCustomer_TextChanged(object sender, EventArgs e)
        {
            // Tìm bằng mã hoặc số điện thoại
            string searchText = txtCustomer.Text.Trim();
            using (var context = new salesysdbEntities())
            {
                CustomerBUS customerBUS = new CustomerBUS(context);
                Customer customers = customerBUS.GetCustomerByIdOrPhone(searchText);

                // Nếu có kết quả, hiển thị thông tin tên
                if (customers != null)
                    txtCustomerName.Text = customers.FullName;
                else
                    txtCustomerName.Clear();
            }

        }

        private void btnFindCustomer_Click(object sender, EventArgs e)
        {
            using (var findCustomerForm = new FindCustomerForm())
            {
                if (findCustomerForm.ShowDialog() == DialogResult.OK)
                {
                    if (findCustomerForm.Tag is Customer selectedCustomer)
                    {
                        txtCustomer.Text = selectedCustomer.CustomerID.ToString();
                        txtCustomerName.Text = selectedCustomer.FullName;
                    }
                }
            }

        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            // chỉ cho số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtAmountPaid_KeyPress(object sender, KeyPressEventArgs e)
        {
            // chỉ cho số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtAmountPaid_TextChanged(object sender, EventArgs e)
        {
            // Tính toán tiền thừa
            if (long.TryParse(txtAmountPaid.Text, out long amountPaid) && long.TryParse(txtAmount.Text, out long amount))
            {
                long change = amountPaid - amount;
                txtCashChange.Text = change >= 0 ? change.ToString("N0") : "0";
            }
            else
            {
                txtCashChange.Text = "0";
            }

        }

        private void btnAddToInvoice_Click(object sender, EventArgs e)
        {
            if (cbProductName.SelectedItem is Product selectedProduct && 
                int.TryParse(txtAmount.Text, out int quantity))
            {
                long unitPrice = selectedProduct.UnitPrice;
                // Tạo một dòng mới trong DataGridView
                var newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridView);
                newRow.Cells[0].Value = selectedProduct.ProductID;
                newRow.Cells[1].Value = selectedProduct.ProductName;
                newRow.Cells[2].Value = quantity;
                newRow.Cells[3].Value = unitPrice;
                newRow.Cells[4].Value = quantity * unitPrice; // Tổng tiền
                dataGridView.Rows.Add(newRow);
                // Cập nhật tổng tiền
                UpdateTotalAmount();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn sản phẩm và nhập số lượng và giá.");
            }

        }
        private void UpdateTotalAmount()
        {
            long totalAmount = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[4].Value != null && long.TryParse(row.Cells[4].Value.ToString(), out long rowTotal))
                {
                    totalAmount += rowTotal;
                }
            }
            labelTotalCost.Text += totalAmount.ToString("N0");
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 0)
            {
                MessageBox.Show("Không có sản phẩm nào trong hóa đơn.");
                return;
            }
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
            PrintDialog printDialog = new PrintDialog
            {
                Document = printDocument
            };
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Thiết lập font và kích thước
            Font font = new Font("Arial", 12);
            float lineHeight = font.GetHeight(e.Graphics);
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            // In tiêu đề hóa đơn
            e.Graphics.DrawString("HÓA ĐƠN BÁN HÀNG", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, x, y);
            y += lineHeight * 2;
            // In thông tin khách hàng
            e.Graphics.DrawString($"Khách hàng: {txtCustomerName.Text}", font, Brushes.Black, x, y);
            y += lineHeight;
            // In thông tin sản phẩm
            e.Graphics.DrawString("Sản phẩm:", font, Brushes.Black, x, y);
            y += lineHeight;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    string productInfo = $"{row.Cells[1].Value} - Số lượng: {row.Cells[2].Value} - Đơn giá: {row.Cells[3].Value} - Tổng: {row.Cells[4].Value}";
                    e.Graphics.DrawString(productInfo, font, Brushes.Black, x, y);
                    y += lineHeight;
                }
            }
            // In tổng tiền
            e.Graphics.DrawString($"Tổng tiền: {labelTotalCost.Text}", font, Brushes.Black, x, y);
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {

        }
    }
}
