using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class InvoiceDetailDAL
    {
        private readonly salesysdbEntities context;
        public InvoiceDetailDAL(salesysdbEntities context)
        {
            this.context = context;
        }
        public List<InvoiceDetail> GetInvoiceDetails(int invoiceId)
        {
            return context.InvoiceDetails.Where(id => id.InvoiceID == invoiceId).ToList();
        }
        public void AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            context.InvoiceDetails.Add(invoiceDetail);
            context.SaveChanges();
        }
        public void UpdateInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            var existingDetail = context.InvoiceDetails.Find(invoiceDetail.InvoiceID);
            if (existingDetail != null)
            {
                existingDetail.Quantity = invoiceDetail.Quantity;
                existingDetail.UnitPrice = invoiceDetail.UnitPrice;
                existingDetail.ProductID = invoiceDetail.ProductID;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Invoice detail not found");
            }
        }
    }
}
