using Entity;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;

namespace BusinessLayer
{
    public class InvoiceBUS
    {
        private readonly InvoiceDAL invoiceDAL;
        public InvoiceBUS(salesysdbEntities context)
        {
            this.invoiceDAL = new InvoiceDAL(context);
        }
        public List<Invoice> GetInvoicesFiltered(string keyword, string employeeName, bool? deliveryRequired, DateTime? startDate, DateTime? endDate)
        {
            return invoiceDAL.GetInvoicesFiltered(keyword, employeeName, deliveryRequired, startDate, endDate);

        }
        public Invoice GetInvoiceById(int invoiceId)
        {
            if (invoiceId <= 0)
            {
                throw new ArgumentException("Mã đơn hàng không được bé hơn 0", nameof(invoiceId));
            }
            return invoiceDAL.GetInvoiceById(invoiceId);
        }
        public void AddInvoice(Invoice invoice)
        {
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice), "Mã đơn hàng không được null");
            }
            invoiceDAL.AddInvoice(invoice);
        }
        public void recalculateTotalAmount(int invoiceId)
        {
            if (invoiceId <= 0)
            {
                throw new ArgumentException("Mã đơn hàng không được bé hơn 0", nameof(invoiceId));
            }
            invoiceDAL.RecalculateTotalAmount(invoiceId);
        }

        public void UpdateStatus(Invoice invoice)
        {
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice), "Hóa đơn không được null");
            }
            invoiceDAL.UpdateStatus(invoice);
        }

    }
}
