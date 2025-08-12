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

namespace PresentationLayer
{
    public partial class StockInManagementForm : Form
    {
        private const String DEFAULT_SEARCH_TEXT = "Tìm kiếm theo tên sản phẩm hoặc email nhân viên hoặc tên nhân viên...";
        private readonly StockInBUS stockInBUS;
        private Timer debounceTimer;
        public StockInManagementForm()
        {
            stockInBUS = new StockInBUS();
            InitializeComponent();
            debounceTimer = new Timer
            {
                Interval = 300
            };
            debounceTimer.Tick += DebounceTimer_Tick;
            gridViewStockIn.CellContentClick += GridViewUserAccount_CellContentClick;
            gridViewStockIn.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            loadData();
        }

        public void loadData()
        {
            gridViewStockIn.Columns.Clear();
            gridViewStockIn.DataSource = null;

            txtSearch.Text = DEFAULT_SEARCH_TEXT;
            txtSearch.ForeColor = Color.Gray;

            gridViewStockIn.DataSource = stockInBUS.Get(null);

            gridViewStockIn.Columns["Employee"].Visible = false;
            gridViewStockIn.Columns["StockInDetails"].Visible = false;
            gridViewStockIn.Columns["TotalAmount"].DefaultCellStyle.Format = "N0";
            var btnEdit = new DataGridViewButtonColumn
            {
                Name = "btnEdit",
                HeaderText = "StockInDetails",
                Text = "Chỉnh sửa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewStockIn.Columns.Add(btnEdit);

            var btnDelete = new DataGridViewButtonColumn
            {
                Name = "btnDelete",
                HeaderText = "",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewStockIn.Columns.Add(btnDelete);
        }

        public void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            string keyword = txtSearch.Text.Trim();
            if (keyword == DEFAULT_SEARCH_TEXT || string.IsNullOrEmpty(keyword))
            {
                gridViewStockIn.DataSource = stockInBUS.Get(null);
            }
            else
            {
                gridViewStockIn.DataSource = stockInBUS.Get(keyword);
            }
        }

        private void GridViewUserAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = gridViewStockIn.Columns[e.ColumnIndex].Name;

            var stockInID = gridViewStockIn.Rows[e.RowIndex].Cells["StockInID"].Value;

            if (columnName == "btnEdit")
            {
                StockInForm sForm = new StockInForm((int)stockInID);
                sForm.ShowDialog();

                loadData();

            }
            else if (columnName == "btnDelete")
            {
                var confirm = MessageBox.Show($"Xác nhận đơn nhập hàng với ID: {stockInID}?\nLưu ý: Chi tiết đơn nhập hàng cũng sẽ bị xóa theo!", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    stockInBUS.Delete((int)stockInID);
                    MessageBox.Show("Xóa đơn nhập hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                }
            }
        }

        private void btnAddStockIn_Click(object sender, EventArgs e)
        {
            StockInForm sForm = new StockInForm(-1);
            sForm.ShowDialog();

            loadData();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            debounceTimer.Start();
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
    }
}
