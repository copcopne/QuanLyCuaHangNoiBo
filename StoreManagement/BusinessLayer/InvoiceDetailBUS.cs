using Entity;
using System;
using DataAccessLayer;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class InvoiceDetailBUS
    {
        private readonly InvoiceDetailDAL invoiceDetailDAL;
        public InvoiceDetailBUS(salesysdbEntities context)
        {
            this.invoiceDetailDAL = new DataAccessLayer.InvoiceDetailDAL(context);
        }
        public List<InvoiceDetail> GetInvoiceDetails(int invoiceId)
        {
            return invoiceDetailDAL.GetInvoiceDetails(invoiceId);
        }
        public void AddInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            if (invoiceDetail == null)
            {
                throw new ArgumentNullException(nameof(invoiceDetail), "Chi tiết hóa đơn không được null");
            }
            invoiceDetailDAL.AddInvoiceDetail(invoiceDetail);
        }
        public void UpdateInvoiceDetail(InvoiceDetail invoiceDetail)
        {
            if (invoiceDetail == null)
            {
                throw new ArgumentNullException(nameof(invoiceDetail), "Chi tiết hóa đơn không được null");
            }
            invoiceDetailDAL.UpdateInvoiceDetail(invoiceDetail);
        }
    }
}
