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
    public partial class EmployeeManagementForm : Form
    {

        private const String DEFAULT_SEARCH_TEXT = "Tìm kiếm theo tên hoặc email...";
        private readonly EmployeeBUS employeeBUS;
        private Timer debounceTimer;
        public EmployeeManagementForm()
        {
            employeeBUS = new EmployeeBUS();
            InitializeComponent();

            this.gridViewEmployee.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridViewEmployee.CellContentClick += EmployeeGridView_CellContentClick;
            debounceTimer = new Timer
            {
                Interval = 300
            };
            debounceTimer.Tick += DebounceTimer_Tick;
        }

        public void loadData()
        {
            this.gridViewEmployee.Columns.Clear();
            this.gridViewEmployee.DataSource = null;

            this.txtSearch.Text = String.IsNullOrEmpty(txtSearch.Text) ? DEFAULT_SEARCH_TEXT : txtSearch.Text;
            this.txtSearch.ForeColor = Color.Gray;

            this.gridViewEmployee.DataSource = employeeBUS.Get(null);

            this.gridViewEmployee.Columns["UserAccount"].Visible = false;
            this.gridViewEmployee.Columns["Deliveries"].Visible = false;
            this.gridViewEmployee.Columns["Invoices"].Visible = false;
            this.gridViewEmployee.Columns["StockIns"].Visible = false;
            this.gridViewEmployee.Columns["StockRequests"].Visible = false;

            var btnUserAcc = new DataGridViewButtonColumn
            {
                Name = "btnUserAcc",
                HeaderText = "UserAccount",
                Text = "Chỉnh sửa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewEmployee.Columns.Add(btnUserAcc);
            var btnEdit = new DataGridViewButtonColumn
            {
                Name = "btnEdit",
                HeaderText = "",
                Text = "Chỉnh sửa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewEmployee.Columns.Add(btnEdit);

            var btnDelete = new DataGridViewButtonColumn
            {
                Name = "btnDelete",
                HeaderText = "",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 80
            };
            gridViewEmployee.Columns.Add(btnDelete);
        }

        public void DebounceTimer_Tick(object sender, EventArgs e)
        {
            debounceTimer.Stop();
            string keyword = txtSearch.Text.Trim();
            if (keyword == DEFAULT_SEARCH_TEXT || string.IsNullOrEmpty(keyword))
            {
                gridViewEmployee.DataSource = employeeBUS.Get(null);
            }
            else
            {
                gridViewEmployee.DataSource = employeeBUS.Get(keyword);
            }
        }

        private void EmployeeManagementForm_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void EmployeeGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string columnName = gridViewEmployee.Columns[e.ColumnIndex].Name;

            var employeeId = gridViewEmployee.Rows[e.RowIndex].Cells["EmployeeID"].Value;

            if (columnName == "btnEdit")
            {
                MessageBox.Show($"Sửa nhân viên ID: {employeeId}");
                EmployeeForm eForm = new EmployeeForm((int)employeeId);
                eForm.ShowDialog();

                loadData();

            }
            else if (columnName == "btnDelete")
            {
                var confirm = MessageBox.Show($"Xóa nhân viên ID: {employeeId}?", "Xác nhận", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    //employeeBUS.Delete((int)employeeId);
                    //employeeGridView.DataSource = employeeBUS.Get(null); // refresh
                }
            }
            else if (columnName == "btnUserAcc")
            {
                
            }
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

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            EmployeeForm eForm = new EmployeeForm(-1);
            eForm.ShowDialog();
        }
    }
}
