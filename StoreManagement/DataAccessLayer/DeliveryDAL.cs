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
        public List<Delivery> GetDeliveries()
        {
            return context.Deliveries.ToList();
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
    }
}
