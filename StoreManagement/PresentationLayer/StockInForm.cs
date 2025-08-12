using Entity;
using System;
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
    public partial class StockInForm : Form
    {
        private List<Entity.StockInDetail> stockInDetails;
        private BusinessLayer.StockInBUS stockInBUS;
        private BusinessLayer.ProductBUS productBUS;
        private Entity.StockIn stockIn;
        private Boolean isEditMode = false;
        public StockInForm(int stockInID)
        {
            stockInBUS = new BusinessLayer.StockInBUS();
            productBUS = new BusinessLayer.ProductBUS(new salesysdbEntities());
            InitializeComponent();
            gridViewDetails.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridViewDetails.CellContentClick += GridViewDetails_CellContentClick;
            if (stockInID > 0)
            {
                stockIn = stockInBUS.Get(stockInID);
                if (stockIn != null)
                {

                }
                else
                {

                }
            }
            else
            {
                stockIn = new Entity.StockIn();
                stockIn.EmployeeID = 1;
                stockInDetails = new List<Entity.StockInDetail>();
            }
            load();
        }

        public void load()
        {
            // Grid
            refreshDetailsGrid();
            if (gridViewDetails.Columns.Contains("StockInID")) gridViewDetails.Columns["StockInID"].Visible = false;
            if (gridViewDetails.Columns.Contains("Product")) gridViewDetails.Columns["Product"].Visible = false;
            if (gridViewDetails.Columns.Contains("StockIn")) gridViewDetails.Columns["StockIn"].Visible = false;

            // Combo
            var products = productBUS.GetProducts(null) ?? new List<Entity.Product>();
            products.Insert(0, new Entity.Product { ProductID = 0, ProductName = "---Chọn sản phẩm---" });

            cmbProducts.SelectedIndexChanged -= cmbProducts_SelectedIndexChanged;
            cmbProducts.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbProducts.DataSource = products;
            cmbProducts.DisplayMember = "ProductName";
            cmbProducts.ValueMember = "ProductID";
            cmbProducts.SelectedIndex = products.Count > 0 ? 0 : -1;
            cmbProducts.SelectedIndexChanged += cmbProducts_SelectedIndexChanged;

            txtName.Text = txtPrice.Text = txtUnit.Text = txtMinStock.Text = txtQuantity.Text = "";
        }

        private void StockInForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {

            ProductForm pForm = new ProductForm();
            pForm.ShowDialog();

            load();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (stockInDetails.Count == 0)
            {
                MessageBox.Show("Vui lòng thêm ít nhất một sản phẩm vào danh sách nhập hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn lưu thông tin nhập hàng?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            try
            {
                stockInBUS.AddStockIn(stockIn, stockInDetails);
                MessageBox.Show("Lưu thông tin nhập hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thông tin nhập hàng: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if(isEditMode)
            {
                isEditMode = false;
                cmbProducts.Enabled = true;
                cmbProducts.SelectedIndex = 0;
                btnCancel.Text = "Hủy";
                btnAdd.Text = "Thêm SP";
                txtQuantity.Clear();
                txtQuantity.Focus();
            }
            else
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn hủy thao tác?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
                this.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedIndex <= 0)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Vui lòng nhập số lượng hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                txtQuantity.SelectAll();
                return;
            }
            var p = cmbProducts.SelectedItem as Entity.Product;
            if (p == null) return;

            var existed = stockInDetails.FirstOrDefault(d => d.ProductID == p.ProductID);
            if (existed != null)
            {
                if (isEditMode)
                {
                    isEditMode = false;
                    cmbProducts.Enabled = true;
                    cmbProducts.SelectedIndex = 0;
                    existed.Quantity = quantity;
                    btnCancel.Text = "Hủy";
                    btnAdd.Text = "Cập nhật";
                }
                else
                {
                    existed.Quantity += quantity;
                }
            }
            else
            {
                stockInDetails.Add(new Entity.StockInDetail
                {
                    ProductID = p.ProductID,
                    Quantity = quantity,
                    UnitCost = p.UnitPrice,
                    Product = p
                });
            }

            load();

            txtQuantity.Clear();
            txtQuantity.Focus();
        }

        private void refreshDetailsGrid()
        {
            gridViewDetails.Columns.Clear();
            gridViewDetails.DataSource = null;
            gridViewDetails.DataSource = stockInDetails
                .Select(d => new
                {
                    d.ProductID,
                    ProductName = d.Product?.ProductName,
                    d.Quantity,
                    d.UnitCost
                })
                .ToList();

            var btnEdit = new DataGridViewButtonColumn
            {
                Name = "btnEdit",
                HeaderText = "",
                Text = "Chỉnh sửa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewDetails.Columns.Add(btnEdit);

            var btnDelete = new DataGridViewButtonColumn
            {
                Name = "btnDelete",
                HeaderText = "",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewDetails.Columns.Add(btnDelete);

        }

        private void cmbProducts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProducts.SelectedIndex < 0) return;

            if (cmbProducts.SelectedIndex == 0) // "---Chọn sản phẩm---"
            {
                txtName.Text = txtPrice.Text = txtUnit.Text = txtMinStock.Text = "";
                return;
            }

            var p = cmbProducts.SelectedItem as Entity.Product;
            if (p == null) return;

            txtName.Text = p.ProductName;
            txtPrice.Text = p.UnitPrice.ToString();
            txtUnit.Text = p.Unit;
            txtMinStock.Text = p.MinStockLevel.ToString();
            txtQuantity.Focus();
        }
        private void GridViewDetails_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string columnName = gridViewDetails.Columns[e.ColumnIndex].Name;

            var productId = gridViewDetails.Rows[e.RowIndex].Cells["ProductID"].Value;

            if (columnName == "btnEdit")
            {
                cmbProducts.SelectedValue = Convert.ToInt32(productId);
                cmbProducts.Enabled = false;
                txtQuantity.Text = gridViewDetails.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                txtQuantity.SelectAll();
                txtQuantity.Focus();
                isEditMode = true;
                btnCancel.Text = "Hủy chỉnh sửa";
                btnAdd.Text = "Cập nhật";

            }
            else if (columnName == "btnDelete")
            {
                var confirm = MessageBox.Show($"Xác nhận xóa sản phẩm với ID: {productId}?", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    var productIdToDelete = Convert.ToInt32(productId);
                    var detailToRemove = stockInDetails.FirstOrDefault(d => d.ProductID == productIdToDelete);
                    if (detailToRemove != null)
                    {
                        stockInDetails.Remove(detailToRemove);
                        MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        load();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }
    }
}
