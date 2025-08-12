using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Entity;

namespace PresentationLayer
{
    public partial class ProductManagementForm : Form
    {
        private const String DEFAULT_SEARCH_TEXT = "Tìm kiếm theo tên sản phẩm...";
        private salesysdbEntities context = new salesysdbEntities();
        private readonly ProductBUS productBUS;
        private Timer debounceTimer;
        public ProductManagementForm()
        {
            InitializeComponent();
            productBUS = new ProductBUS(context);
            debounceTimer = new Timer
            {
                Interval = 300
            };
            debounceTimer.Tick += DebounceTimer_Tick;
            gridViewProducts.CellContentClick += GridViewProduct_CellContentClick;
            gridViewProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            loadData();

        }
        public void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            string keyword = txtSearch.Text.Trim();
            if (keyword == DEFAULT_SEARCH_TEXT || string.IsNullOrEmpty(keyword))
            {
                gridViewProducts.DataSource = productBUS.GetProducts(null);
            }
            else
            {
                gridViewProducts.DataSource = productBUS.GetProducts(keyword);
            }
        }

        public void loadData()
        {
            gridViewProducts.Columns.Clear();
            gridViewProducts.DataSource = productBUS.GetProducts(null);

            txtSearch.Text = DEFAULT_SEARCH_TEXT;
            txtSearch.ForeColor = Color.Gray;


            gridViewProducts.Columns["isDeleted"].Visible = false;
            gridViewProducts.Columns["Category"].Visible = false;
            gridViewProducts.Columns["InvoiceDetails"].Visible = false;
            gridViewProducts.Columns["StockRequestDetails"].Visible = false;
            gridViewProducts.Columns["StockInDetails"].Visible = false;


            var btnEdit = new DataGridViewButtonColumn
            {
                Name = "btnEdit",
                HeaderText = "",
                Text = "Chỉnh sửa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewProducts.Columns.Add(btnEdit);

            var btnDelete = new DataGridViewButtonColumn
            {
                Name = "btnDelete",
                HeaderText = "",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewProducts.Columns.Add(btnDelete);

        }

        private void btnStockIn_Click(object sender, EventArgs e)
        {
            StockInForm sForm = new StockInForm(0);
            sForm.ShowDialog();

            loadData();
        }

        private void btnStockRequest_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            this.txtSearch.ForeColor = Color.Black;
            this.txtSearch.Text = "";
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtSearch.Text))
            {
                this.txtSearch.ForeColor = Color.Gray;
                this.txtSearch.Text = DEFAULT_SEARCH_TEXT;
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
        }

        private void ProductManagementForm_Load(object sender, EventArgs e)
        {

        }

        private void GridViewProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string columnName = gridViewProducts.Columns[e.ColumnIndex].Name;

            var productId = gridViewProducts.Rows[e.RowIndex].Cells["ProductID"].Value;

            if (columnName == "btnEdit")
            {
                ProductForm pForm = new ProductForm(Convert.ToInt32(productId));
                pForm.ShowDialog();

                loadData();

            }
            else if (columnName == "btnDelete")
            {
                var confirm = MessageBox.Show($"Xác nhận xóa sản phẩm với ID: {productId}?", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    productBUS.DeleteProduct(Convert.ToInt32(productId));
                    MessageBox.Show("Xóa sản phẩm thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                }
            }
        }
    }
}
