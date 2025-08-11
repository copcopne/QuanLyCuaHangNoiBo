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
    public partial class InvoiceDetailForm : Form
    {
        private readonly salesysdbEntities context = new salesysdbEntities();
        private readonly InvoiceDetailBUS invoiceDetailBUS;
        private readonly CategoryBUS categoryBUS;
        private readonly ProductBUS productBUS;
        private readonly Invoice invoice;
        private InvoiceDetail invoiceDetail;
        public InvoiceDetailForm(Invoice invoice, int productID)
        {
            InitializeComponent();
            this.invoice = invoice;
            this.invoiceDetailBUS = new InvoiceDetailBUS(context);
            this.categoryBUS = new CategoryBUS(context);
            this.productBUS = new ProductBUS(context);
            this.invoiceDetail = invoice.InvoiceDetails.FirstOrDefault(id => id.ProductID == productID);
        }
        public InvoiceDetailForm()
        {
            InitializeComponent();
            this.invoiceDetailBUS = new InvoiceDetailBUS(context);
        }

        private void InvoiceDetailForm_Load(object sender, EventArgs e)
        {
            labelTitle.Text += $"{invoice.InvoiceID}";
            LoadComboBox();

        }
        private void LoadComboBox()
        {
            if (invoiceDetail != null)
            {
                cbCategory.DataSource = categoryBUS.GetCategories();
                cbCategory.DisplayMember = "CategoryName";
                cbCategory.ValueMember = "CategoryID";
            }
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (invoiceDetail != null)
            {
                int selectedCategoryId = (int)cbCategory.SelectedValue;
                var productsInCategory = productBUS.GetProductsByCategory(selectedCategoryId);
                cbProductName.DataSource = productsInCategory;
                cbProductName.DisplayMember = "ProductName";
                cbProductName.ValueMember = "ProductID";
                
            }
        }
    }
}
