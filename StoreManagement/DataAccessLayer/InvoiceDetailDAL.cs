using Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            return context.InvoiceDetails.Where(id => id.InvoiceID == invoiceId).AsNoTracking().ToList();
        }
        public void AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            context.InvoiceDetails.Add(invoiceDetail);
            context.SaveChanges();
        }
        public void UpdateInvoiceDetail(InvoiceDetail invoiceDetail)
        {

            var existingDetail = context.InvoiceDetails.FirstOrDefault(id => id.InvoiceID == invoiceDetail.InvoiceID && id.ProductID == invoiceDetail.ProductID);
            if (existingDetail != null)
            {
                existingDetail.Quantity = invoiceDetail.Quantity;
                existingDetail.UnitPrice = invoiceDetail.UnitPrice;
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Chi tiết hóa đơn không tồn tại.");
            }
        }
        public void DeleteInvoiceDetail(int invoiceId, int productId)
        {
            var detail = context.InvoiceDetails.FirstOrDefault(id => id.InvoiceID == invoiceId && id.ProductID == productId);
            if (detail != null)
            {
                context.InvoiceDetails.Remove(detail);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Chi tiết hóa đơn không tồn tại.");
            }
        }
    }
}
