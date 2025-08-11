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
    public partial class UserAccountManagementForm : Form
    {
        private const String DEFAULT_SEARCH_TEXT = "Tìm kiếm theo tên người dùng hoặc email nhân viên hoặc tên nhân viên...";
        private readonly UserAccountBUS userAccountBUS;
        private Timer debounceTimer;
        public UserAccountManagementForm()
        {
            userAccountBUS = new UserAccountBUS();
            InitializeComponent();

            gridViewUserAccount.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridViewUserAccount.CellContentClick += GridViewUserAccount_CellContentClick;
            debounceTimer = new Timer
            {
                Interval = 300
            };
            debounceTimer.Tick += DebounceTimer_Tick;
            loadData();
        }
        public void loadData()
        {
            gridViewUserAccount.Columns.Clear();
            gridViewUserAccount.DataSource = null;

            txtSearch.Text = DEFAULT_SEARCH_TEXT;
            txtSearch.ForeColor = Color.Gray;

            gridViewUserAccount.DataSource = userAccountBUS.Get(null);

            gridViewUserAccount.Columns["Employee"].Visible = false;
            gridViewUserAccount.Columns["PasswordHash"].Visible = false;
            var btnEdit = new DataGridViewButtonColumn
            {
                Name = "btnEdit",
                HeaderText = "",
                Text = "Chỉnh sửa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewUserAccount.Columns.Add(btnEdit);

            var btnDelete = new DataGridViewButtonColumn
            {
                Name = "btnDelete",
                HeaderText = "",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewUserAccount.Columns.Add(btnDelete);
        }

        public void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            string keyword = txtSearch.Text.Trim();
            if (keyword == DEFAULT_SEARCH_TEXT || string.IsNullOrEmpty(keyword))
            {
                gridViewUserAccount.DataSource = userAccountBUS.Get(null);
            }
            else
            {
                gridViewUserAccount.DataSource = userAccountBUS.Get(keyword);
            }
        }
        private void GridViewUserAccount_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = gridViewUserAccount.Columns[e.ColumnIndex].Name;

            var employeeId = gridViewUserAccount.Rows[e.RowIndex].Cells["EmployeeID"].Value;

            if (columnName == "btnEdit")
            {
                UserForm uForm = new UserForm((int)employeeId);
                uForm.ShowDialog();

                loadData();

            }
            else if (columnName == "btnDelete")
            {
                var confirm = MessageBox.Show($"Xác nhận xóa tài khoản với ID: {employeeId}?", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    userAccountBUS.Delete((int)employeeId);
                    MessageBox.Show("Xóa tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadData();
                }
            }
            else if (columnName == "btnUserAcc")
            {
                UserForm uForm = new UserForm((int)employeeId);
                uForm.ShowDialog();

                loadData();
            }
        }

        private void btnAddUserAccount_Click(object sender, EventArgs e)
        {

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
