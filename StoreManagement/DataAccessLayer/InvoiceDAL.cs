using System;
using Entity;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class InvoiceDAL
    {
        private readonly DeliveryDAL deliveryDAL;
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
        public Invoice AddInvoice(Invoice invoice)
        {
            if (invoice == null)
                throw new ArgumentNullException(nameof(invoice), "Hóa đơn không được null");

            if (invoice.InvoiceDetails == null || !invoice.InvoiceDetails.Any())
                throw new ArgumentException("Hóa đơn phải có ít nhất một chi tiết.", nameof(invoice));

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    invoice.InvoiceDate = DateTime.Now;
                    context.Invoices.Add(invoice);
                    context.SaveChanges();
                    // Cập nhật số lượng tồn kho cho từng chi tiết hóa đơn
                    foreach (var detail in invoice.InvoiceDetails)
                    {
                        var product = context.Products.Find(detail.ProductID);
                        if (product == null)
                        {
                            throw new ArgumentException($"Sản phẩm với ID {detail.ProductID} không tồn tại.");
                        }
                        if (product.StockQuantity < detail.Quantity)
                        {
                            throw new InvalidOperationException($"Số lượng tồn kho không đủ cho sản phẩm {product.ProductName}.");
                        }
                        product.StockQuantity -= detail.Quantity;
                    }
                    context.SaveChanges();
                    transaction.Commit();
                    return invoice;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
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
            return query.AsNoTracking().ToList();
        }
        public void RecalculateTotalAmount(int invoiceId)
        {
            var invoice = context.Invoices.Include(i => i.InvoiceDetails).FirstOrDefault(i => i.InvoiceID == invoiceId);
            if (invoice == null)
            {
                throw new ArgumentException("Hóa đơn không tồn tại", nameof(invoiceId));
            }
            long totalAmount = invoice.InvoiceDetails.Sum(id => id.Quantity * id.UnitPrice);
            invoice.TotalAmount = totalAmount;
            context.SaveChanges();
        }

        public void UpdateInvoice(Invoice invoice)
        {
            // Chỉ cập nhật thông tin chung
            var existingInvoice = context.Invoices.Include(i => i.InvoiceDetails).FirstOrDefault(i => i.InvoiceID == invoice.InvoiceID);
            if (existingInvoice == null)
            {
                throw new ArgumentException("Hóa đơn không tồn tại", nameof(invoice.InvoiceID));
            }
            existingInvoice.CustomerID = invoice.CustomerID;
            existingInvoice.DeliveryRequired = invoice.DeliveryRequired;
            context.SaveChanges();
        }
    }
}
