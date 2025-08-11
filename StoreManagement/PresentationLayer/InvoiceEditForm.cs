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
    public partial class InvoiceEditForm : Form
    {
        public InvoiceEditForm(Invoice invoice)
        {
            InitializeComponent();
        }
    }
}
