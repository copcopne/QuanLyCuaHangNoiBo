using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DataAccessLayer
{
    internal class InvoiceDAL
    {
        DeliveryDAL deliveryDAL;
        private readonly salesysdbEntities context;
        public InvoiceDAL()
        {
            this.context = new salesysdbEntities();
            this.deliveryDAL = new DeliveryDAL(context);
        }
        public List<Invoice> GetInvoices()
        {
            return context.Invoices.ToList();
        }
        public void AddInvoice(Invoice invoice)
        {
            context.Invoices.Add(invoice);
            context.SaveChanges();
        }
    }
}
