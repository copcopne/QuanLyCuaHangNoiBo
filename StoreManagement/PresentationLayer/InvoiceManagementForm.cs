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
    public partial class InvoiceManagementForm : Form
    {
        private readonly salesysdbEntities context = new salesysdbEntities();
        private readonly InvoiceBUS invoiceService;

        public InvoiceManagementForm()
        {
            InitializeComponent();
            this.invoiceService = new InvoiceBUS(context);
        }

        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void InvoiceManagement_Load(object sender, EventArgs e)
        {
            dataGridView.DataSource = invoiceService.GetInvoices();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // ẩn cột không cần thiết
            dataGridView.Columns["Customer"].Visible = false;
            dataGridView.Columns["Deliveries"].Visible = false;
            dataGridView.Columns["Employee"].Visible = false;
            dataGridView.Columns["InvoiceDetails"].Visible = false;


        }
    }
}
