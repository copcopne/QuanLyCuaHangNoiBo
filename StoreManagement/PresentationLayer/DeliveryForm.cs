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
    public partial class DeliveryForm : Form
    {
        private readonly salesysdbEntities context;
        private readonly DeliveryBUS deliveryBUS;
        private readonly InvoiceBUS invoiceBUS;
        private Invoice invoice;
        public DeliveryForm(int invoiceId)
        {
            InitializeComponent();
            this.context = new salesysdbEntities();
            this.deliveryBUS = new DeliveryBUS(context);
            this.invoiceBUS = new InvoiceBUS(context);
            this.invoice = invoiceBUS.GetInvoiceById(invoiceId);
            if (invoice == null)
            {
                MessageBox.Show("Hóa đơn không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
        }
        public DeliveryForm()
        {
            InitializeComponent();
            this.context = new salesysdbEntities();
            this.deliveryBUS = new DeliveryBUS(context);
            this.invoiceBUS = new InvoiceBUS(context);
        }

        private void DeliveryForm_Load(object sender, EventArgs e)
        {

        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ giao hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (invoice == null)
            {
                // Nếu không có hóa đơn, chỉ trả địa chỉ và ghi chú
                Tag = new
                {
                    Address = txtAddress.Text.Trim(),
                    Notes = txtNote.Text.Trim()
                };
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                deliveryBUS.AddDelivery(new Delivery
                {
                    InvoiceID = invoice.InvoiceID,
                    DeliveryAddress = txtAddress.Text.Trim(),
                    Notes = txtNote.Text.Trim(),
                    Status = "Chưa phân công",
                });
                deliveryBUS.AutoAssignDelivery();
                this.DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
