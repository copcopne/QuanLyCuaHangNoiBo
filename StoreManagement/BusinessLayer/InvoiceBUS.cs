using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class InvoiceBUS
    {
        private readonly DataAccessLayer.InvoiceDAL invoiceDAL;
        public InvoiceBUS(salesysdbEntities context)
        {
            this.invoiceDAL = new DataAccessLayer.InvoiceDAL(context);
        }
        public List<Entity.Invoice> GetInvoices()
        {
            return invoiceDAL.GetInvoices();
        }
        public void AddInvoice(Entity.Invoice invoice)
        {
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice), "Invoice cannot be null");
            }
            invoiceDAL.AddInvoice(invoice);
        }
    }
}
