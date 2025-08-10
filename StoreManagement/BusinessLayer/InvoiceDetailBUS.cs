using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class InvoiceDetailBUS
    {
        private readonly DataAccessLayer.InvoiceDetailDAL invoiceDetailDAL;
        public InvoiceDetailBUS(salesysdbEntities context)
        {
            this.invoiceDetailDAL = new DataAccessLayer.InvoiceDetailDAL(context);
        }
        public List<Entity.InvoiceDetail> GetInvoiceDetails(int invoiceId)
        {
            return invoiceDetailDAL.GetInvoiceDetails(invoiceId);
        }
        public void AddInvoiceDetail(Entity.InvoiceDetail invoiceDetail)
        {
            if (invoiceDetail == null)
            {
                throw new ArgumentNullException(nameof(invoiceDetail), "Invoice detail cannot be null");
            }
            invoiceDetailDAL.AddInvoiceDetail(invoiceDetail);
        }
        public void UpdateInvoiceDetail(Entity.InvoiceDetail invoiceDetail)
        {
            if (invoiceDetail == null)
            {
                throw new ArgumentNullException(nameof(invoiceDetail), "Invoice detail cannot be null");
            }
            invoiceDetailDAL.UpdateInvoiceDetail(invoiceDetail);
        }
    }
}
