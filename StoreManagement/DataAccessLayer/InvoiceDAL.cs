using System;
using Entity;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DataAccessLayer
{
    public class InvoiceDAL
    {
        DeliveryDAL deliveryDAL;
        private readonly salesysdbEntities context;
        public InvoiceDAL(salesysdbEntities context)
        {
            this.context = new salesysdbEntities();
            this.deliveryDAL = new DeliveryDAL(context);
        }
        public Invoice GetInvoiceById(int invoiceId)
        {
            return context.Invoices.Include(i => i.Customer).Include(i => i.Employee).FirstOrDefault(i => i.InvoiceID == invoiceId);
        }
        public void AddInvoice(Invoice invoice)
        {
            context.Invoices.Add(invoice);
            context.SaveChanges();
        }

        public List<Invoice> GetInvoicesFiltered(string keyword, string employeeName, bool? deliveryRequired, DateTime? startDate, DateTime? endDate)
        {
            var query = context.Invoices.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(i => i.Customer.FullName.Contains(keyword) || i.Employee.FullName.Contains(keyword) || i.InvoiceID.ToString().Contains(keyword));
            }
            if (!string.IsNullOrEmpty(employeeName))
            {
                query = query.Where(i => i.Employee.FullName == employeeName);
            }
            if (deliveryRequired.HasValue)
            {
                query = query.Where(i => i.DeliveryRequired == deliveryRequired.Value);
            }
            if (startDate.HasValue)
            {
                query = query.Where(i => i.InvoiceDate >= startDate.Value);
            }
            if (endDate.HasValue)
            {
                query = query.Where(i => i.InvoiceDate <= endDate.Value);
            }
            return query.ToList();
        }

    }
}
