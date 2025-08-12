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

        }
    }
}
