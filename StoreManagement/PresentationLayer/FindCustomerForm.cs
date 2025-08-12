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
    public partial class FindCustomerForm : Form
    {
        private CustomerBUS customerBUS;
        public FindCustomerForm()
        {
            InitializeComponent();
        }

        private void FindCustomerForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            using (salesysdbEntities context = new salesysdbEntities())
            {
                customerBUS = new CustomerBUS(context);
                List<Customer> customers = customerBUS.GetCustomers(null);
                dataGridView.DataSource = customers;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Lấy khách hàng đã chọn
                Customer selectedCustomer = dataGridView.SelectedRows[0].DataBoundItem as Customer;
                if (selectedCustomer != null)
                {
                    // Trả về khách hàng đã chọn
                    this.DialogResult = DialogResult.OK;
                    this.Tag = selectedCustomer; // Lưu khách hàng đã chọn vào Tag
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một khách hàng.");
            }
        }


        // Trả về khách hàng đã chọn khi chọn một hàng trong DataGridView

    }
}
