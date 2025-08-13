using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DataAccessLayer
{
    public class DeliveryDAL
    {
        private readonly salesysdbEntities context;
        public DeliveryDAL(salesysdbEntities context)
        {
            this.context = context;
        }
        public void AddDelivery(Delivery delivery)
        {
            context.Deliveries.Add(delivery);
            context.SaveChanges();
        }
        public void UpdateDelivery(Delivery delivery)
        {
            var existingDelivery = context.Deliveries.FirstOrDefault(d => d.DeliveryID == delivery.DeliveryID);
        }

        public void DeleteDelivery(Delivery delivery) {
            var existingDelivery = context.Deliveries.FirstOrDefault(d => d.DeliveryID == delivery.DeliveryID);
            if (existingDelivery != null)
            {
                context.Deliveries.Remove(existingDelivery);
                context.SaveChanges();
            }
        }
        public List<Delivery> GetDeliveries(string keyword, DateTime? fromDate, DateTime? toDate, string status)
        {
            var query = context.Deliveries.AsQueryable();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(d => d.DeliveryAddress.Contains(keyword) ||
                                         d.Employee.FullName.Contains(keyword) ||
                                         d.InvoiceID.ToString().Contains(keyword));
            }
            if (fromDate.HasValue)
            {
                query = query.Where(d => d.DeliveryDate >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(d => d.DeliveryDate <= toDate.Value);
            }
            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(d => d.Status == status);
            }
            return query.ToList();
        }
    }
}
