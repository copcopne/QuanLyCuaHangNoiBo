using BusinessLayer;
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
    public partial class ProductForm : Form
    {
        private Entity.Product product;
        private readonly BusinessLayer.ProductBUS productBUS;
        private readonly salesysdbEntities context = new salesysdbEntities();

        public ProductForm()
        {
            InitializeComponent();
            productBUS = new BusinessLayer.ProductBUS(context);
            // Load categories into the combo box
            var categories = context.Categories.ToList();
            categories.Insert(0, new Entity.Category { CategoryID = 0, CategoryName = "---Chọn danh mục---" });
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";
            this.product = new Entity.Product();
        }
        public ProductForm(int productID)
        {
            InitializeComponent();
            productBUS = new BusinessLayer.ProductBUS(context);
            // Load categories into the combo box
            var categories = context.Categories.ToList();
            categories.Insert(0, new Entity.Category { CategoryID = 0, CategoryName = "---Chọn danh mục---" });

            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "CategoryName";
            cmbCategory.ValueMember = "CategoryId";

            if (productID > 0)
            {
                this.product = productBUS.GetProductById(productID);
                if (this.product != null)
                {
                    txtName.Text = this.product.ProductName;
                    txtPrice.Text = this.product.UnitPrice.ToString();
                    txtUnit.Text = this.product.Unit;
                    txtMinStockLevel.Text = this.product.MinStockLevel.ToString();
                    cmbCategory.SelectedValue = this.product.CategoryID;
                }
            }
            else
            {
                this.product = new Entity.Product();
            }
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text) ||
                string.IsNullOrWhiteSpace(txtUnit.Text) ||
                string.IsNullOrWhiteSpace(txtMinStockLevel.Text) ||
                cmbCategory.SelectedIndex == 0)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin sản phẩm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                this.product.ProductName = txtName.Text.Trim();
                this.product.UnitPrice = long.Parse(txtPrice.Text.Trim());
                this.product.Unit = txtUnit.Text.Trim();
                this.product.CategoryID = (int)cmbCategory.SelectedValue;
                this.product.MinStockLevel = int.Parse(txtMinStockLevel.Text.Trim());
                if (this.product.ProductID > 0)
                {
                    productBUS.UpdateProduct(this.product);
                    MessageBox.Show("Cập nhật sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    productBUS.AddProduct(this.product);
                    MessageBox.Show("Thêm sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
