using BusinessLayer;
using Entity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace PresentationLayer
{
    public partial class InvoiceForm : Form
    {
        private ProductBUS productBUS;
        private InvoiceBUS invoiceBUS;
        private CategoryBUS categoryBUS;
        private Invoice currentInvoice;
        private Customer customer;
        private int printRowIndex = 0;
        private long totalAmount = 0;

        public InvoiceForm()
        {
            InitializeComponent();
            salesysdbEntities context = new salesysdbEntities();
            productBUS = new ProductBUS(context);
            categoryBUS = new CategoryBUS(context);
        }


        private void InvoiceForm_Load(object sender, EventArgs e)
        {
            LoadDataGridView();
            LoadComboBox();
            cbCategory.SelectedIndexChanged += cbCategory_SelectedIndexChanged;
        }

        private void LoadComboBox()
        {
            using (var context = new salesysdbEntities())
            {
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

        private void LoadDataGridView()
        {
            dataGridView.DataSource = null;
            dataGridView.Columns.Add("ProductID", "Mã sản phẩm");
            dataGridView.Columns.Add("ProductName", "Tên sản phẩm");
            dataGridView.Columns.Add("Quantity", "Số lượng");
            dataGridView.Columns.Add("UnitPrice", "Đơn giá");
            dataGridView.Columns.Add("TotalPrice", "Tổng tiền");
            dataGridView.Columns["TotalPrice"].DefaultCellStyle.Format = "N0";
            dataGridView.Columns["UnitPrice"].DefaultCellStyle.Format = "N0";
            // Tạo nút bỏ
            var btnRemove = new DataGridViewButtonColumn
            {
                Name = "btnRemove",
                HeaderText = "",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            dataGridView.Columns.Add(btnRemove);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.CellContentClick += DataGridView_CellContentClick;
        }
        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dataGridView.Rows.Count) return;
            if (dataGridView.Columns[e.ColumnIndex].Name == "btnRemove")
            {
                dataGridView.Rows.RemoveAt(e.RowIndex);
                UpdateTotalAmount();
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
                this.customer = customerBUS.GetCustomerByIdOrPhone(searchText);

                // Nếu có kết quả, hiển thị thông tin tên
                if (this.customer != null)
                    txtCustomerName.Text = this.customer.FullName;
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
                        this.customer = selectedCustomer;
                        txtCustomer.Text = this.customer.CustomerID.ToString();
                        txtCustomerName.Text = this.customer.FullName;
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
            if (long.TryParse(txtAmountPaid.Text, out long amountPaid))
            {
                long change = amountPaid - totalAmount;
                txtCashChange.Text = change.ToString("N0");
            }
            else
            {
                txtCashChange.Clear();
            }
        }

        private void btnAddToInvoice_Click(object sender, EventArgs e)
        {
            if (cbProductName.SelectedItem is Product selectedProduct && 
                int.TryParse(txtAmount.Text, out int quantity))
            {
                if (quantity <= 0)
                {
                    MessageBox.Show("Số lượng phải lớn hơn 0.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (selectedProduct.StockQuantity < quantity)
                {
                    MessageBox.Show($"Số lượng vượt quá tồn kho. Tồn kho hiện tại: {selectedProduct.StockQuantity}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                // Nếu tồn tại sẵn trong DataGridView, cập nhật số lượng và giá + thông báo nếu số lượng vượt quá tồn kho
                foreach (DataGridViewRow row in dataGridView.Rows)
                {
                    if (row.Cells["ProductID"].Value != null && 
                        Convert.ToInt32(row.Cells["ProductID"].Value) == selectedProduct.ProductID)
                    {

                        int existingQuantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                        if (existingQuantity + quantity > selectedProduct.StockQuantity)
                        {
                            MessageBox.Show($"Số lượng vượt quá tồn kho. Tồn kho hiện tại: {selectedProduct.StockQuantity}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        row.Cells["Quantity"].Value = existingQuantity + quantity;
                        row.Cells["UnitPrice"].Value = selectedProduct.UnitPrice;
                        row.Cells["TotalPrice"].Value = (existingQuantity + quantity) * selectedProduct.UnitPrice;
                        UpdateTotalAmount();
                        return;
                    }
                }
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
            totalAmount = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells[4].Value != null && long.TryParse(row.Cells[4].Value.ToString(), out long rowTotal))
                {
                    totalAmount += rowTotal;
                }
            }
            labelTotalCost.Text = "Tổng tiền: " + totalAmount.ToString("N0") + " đ";
        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Count == 0 || dataGridView.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("Không có sản phẩm nào trong hóa đơn.");
                return;
            }

            currentInvoice = new Invoice
            {
                InvoiceDate = DateTime.Now,
                Customer = this.customer,
                InvoiceDetails = new List<InvoiceDetail>()
            };

            long total = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;
                var det = new InvoiceDetail();
                det.ProductID = Convert.ToInt32(row.Cells["ProductID"].Value);
                det.Product = new Product { ProductName = row.Cells["ProductName"].Value?.ToString() };
                det.Quantity = Convert.ToInt32(row.Cells["Quantity"].Value);
                det.UnitPrice = Convert.ToInt64(row.Cells["UnitPrice"].Value);
                total += det.Quantity * det.UnitPrice;
                ((List<InvoiceDetail>)currentInvoice.InvoiceDetails).Add(det);
            }
            currentInvoice.TotalAmount = total;

            // reset print index
            printRowIndex = 0;

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;

            PrintPreviewDialog preview = new PrintPreviewDialog { Document = printDocument, Width = 800, Height = 600 };
            // preview.ShowDialog(); // dùng để xem trước
            PrintDialog printDialog = new PrintDialog { Document = printDocument };
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }
        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (currentInvoice == null)
            {
                e.HasMorePages = false;
                return;
            }

            Graphics g = e.Graphics;
            // lấy lề in
            var left = e.MarginBounds.Left;
            var top = e.MarginBounds.Top;
            var right = e.MarginBounds.Right;
            var bottom = e.MarginBounds.Bottom;

            Font fontTitle = new Font("Arial", 14, FontStyle.Bold);
            Font fontHeader = new Font("Arial", 10, FontStyle.Bold);
            Font fontNormal = new Font("Arial", 10);
            Brush brush = Brushes.Black;
            int lineHeight = (int)(fontNormal.GetHeight(g) + 6);
            int y = top;

            // Tiêu đề (ở giữa vùng in)
            string title = "HÓA ĐƠN BÁN HÀNG";
            var titleSize = g.MeasureString(title, fontTitle);
            g.DrawString(title, fontTitle, brush, left + (e.MarginBounds.Width - titleSize.Width) / 2, y);
            y += (int)titleSize.Height + 8;

            // Ngày
            g.DrawString("Ngày: " + currentInvoice.InvoiceDate.ToString("dd/MM/yyyy HH:mm"), fontNormal, brush, left, y);
            y += lineHeight;

            // Khách hàng (nếu có)
            if (currentInvoice.Customer != null)
            {
                g.DrawString("Khách hàng: " + currentInvoice.Customer.FullName, fontNormal, brush, left, y);
                y += lineHeight;
                g.DrawString("Điện thoại: " + currentInvoice.Customer.PhoneNumber, fontNormal, brush, left, y);
                y += lineHeight + 4;
            }
            else
            {
                g.DrawString("Khách hàng: Khách vãng lai", fontNormal, brush, left, y);
                y += lineHeight + 4;
            }

            // Header cột
            int colProductX = left;
            int colQtyX = left + 220;
            int colUnitPriceX = left + 260;
            int colTotalX = left + 360;

            g.DrawString("Sản phẩm", fontHeader, brush, colProductX, y);
            g.DrawString("SL", fontHeader, brush, colQtyX, y);
            g.DrawString("Đơn giá", fontHeader, brush, colUnitPriceX, y);
            g.DrawString("Thành tiền", fontHeader, brush, colTotalX, y);
            y += lineHeight;
            g.DrawLine(Pens.Black, left, y, right, y);
            y += 6;

            // In từng dòng bắt đầu từ printRowIndex
            int startIndex = printRowIndex;
            var details = currentInvoice.InvoiceDetails as IList<InvoiceDetail>;
            for (int i = startIndex; i < details.Count; i++)
            {
                var d = details[i];
                // wrap tên nếu quá dài: cắt đơn giản
                string productName = d.Product?.ProductName ?? "(Sản phẩm)";
                // nếu tên quá dài, bạn có thể dùng MeasureString và DrawString overload để tự xuống dòng
                g.DrawString(productName, fontNormal, brush, colProductX, y);
                // in số (canh phải)
                string qtyText = d.Quantity.ToString();
                string unitPriceText = d.UnitPrice.ToString("N0");
                string totalText = (d.Quantity * d.UnitPrice).ToString("N0");

                // canh phải: vẽ với tính toán width
                var qtyW = g.MeasureString(qtyText, fontNormal).Width;
                g.DrawString(qtyText, fontNormal, brush, colQtyX + (40 - qtyW), y);

                var unitW = g.MeasureString(unitPriceText, fontNormal).Width;
                g.DrawString(unitPriceText, fontNormal, brush, colUnitPriceX + (80 - unitW), y);

                var totW = g.MeasureString(totalText, fontNormal).Width;
                g.DrawString(totalText, fontNormal, brush, colTotalX + (80 - totW), y);

                y += lineHeight;

                // nếu sắp hết trang
                if (y + lineHeight > bottom)
                {
                    printRowIndex = i + 1; // lưu vị trí tiếp theo để in ở trang tiếp
                    e.HasMorePages = true;
                    return;
                }
            }

            // nếu in hết các dòng
            g.DrawLine(Pens.Black, left, y, right, y);
            y += lineHeight;

            // Tổng tiền
            g.DrawString("Tổng tiền: " + currentInvoice.TotalAmount.ToString("N0") + " đ", new Font("Arial", 12, FontStyle.Bold), brush, left, y);
            y += lineHeight + 8;

            // Reset cho lần in kế tiếp
            e.HasMorePages = false;
            printRowIndex = 0;
            // Optionally clear currentInvoice = null;

        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            if (dataGridView.Rows.Cast<DataGridViewRow>().All(r => r.IsNewRow))
            {
                MessageBox.Show("Không có sản phẩm nào trong hóa đơn.");
                return;
            }
            if (!long.TryParse(txtAmountPaid.Text, out long amountPaid) || amountPaid < totalAmount)
            {
                MessageBox.Show("Số tiền đã trả không đủ để thanh toán hóa đơn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo hóa đơn mới
            currentInvoice = new Invoice
            {
                InvoiceDate = DateTime.Now,
                CustomerID = this.customer?.CustomerID,
                InvoiceDetails = new List<InvoiceDetail>(),
                EmployeeID = AuthenticateBUS.CurrentUser.EmployeeID,
            };

            // Tạo chi tiết hóa đơn từ DataGridView
            long total = 0;
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.IsNewRow) continue;

                InvoiceDetail invoiceDetail = new InvoiceDetail
                {
                    InvoiceID = currentInvoice.InvoiceID,
                    ProductID = Convert.ToInt32(row.Cells["ProductID"].Value),
                    Quantity = Convert.ToInt32(row.Cells["Quantity"].Value),
                    UnitPrice = Convert.ToInt64(row.Cells["UnitPrice"].Value)
                };

                total += invoiceDetail.Quantity * invoiceDetail.UnitPrice;
                currentInvoice.InvoiceDetails.Add(invoiceDetail);
            }
            currentInvoice.TotalAmount = total;

            // Lưu
            using (var context = new salesysdbEntities())
            {
                invoiceBUS = new InvoiceBUS(context);
                invoiceBUS.AddInvoice(currentInvoice);
            }

            MessageBox.Show("Hóa đơn đã được tạo thành công!");
            ResetForm();
        }
        private void ResetForm()
        {
            txtCustomer.Clear();
            txtCustomerName.Clear();
            txtAmountPaid.Clear();
            txtCashChange.Clear();
            txtAmount.Clear();
            dataGridView.Rows.Clear();
            labelTotalCost.Text = "Tổng tiền: 0 đ";
            currentInvoice = null;
            customer = null;
        }

        private void cbProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtAmount.Clear();
        }

        private void btnAddNewCustomer_Click(object sender, EventArgs e)
        {
            using (var addCustomerForm = new CustomerForm())
            {
                if (addCustomerForm.ShowDialog() == DialogResult.OK)
                {
                    // Lấy khách hàng mới được thêm
                    this.customer = addCustomerForm.Tag as Customer;
                    txtCustomer.Text = this.customer.CustomerID.ToString();
                    txtCustomerName.Text = this.customer.FullName;
                }
            }
        }
    }
}

