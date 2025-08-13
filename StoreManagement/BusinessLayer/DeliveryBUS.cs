using Entity;
using DataAccessLayer;
using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class DeliveryBUS
    {
        private readonly DeliveryDAL deliveryDAL;
        public DeliveryBUS(salesysdbEntities context)
        {
            this.deliveryDAL = new DataAccessLayer.DeliveryDAL(context);
        }
        public List<Delivery> GetDeliveries(string keyword, DateTime? fromDate, DateTime? toDate, string status)
        {
            return deliveryDAL.GetDeliveries(keyword, fromDate, toDate, status);
        }
        public void AddDelivery(Delivery delivery)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery), "Delivery cannot be null");
            }
            deliveryDAL.AddDelivery(delivery);
        }
    }
}
