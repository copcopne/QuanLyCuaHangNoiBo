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
        public void CancelDelivery(Invoice invoice)
        {
            if (invoice == null)
            {
                throw new ArgumentNullException(nameof(invoice), "Invoice cannot be null");
            }
            deliveryDAL.CancelDelivery(invoice);
        }
        public void UpdateDelivery(Delivery delivery)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery), "Delivery cannot be null");
            }
            deliveryDAL.UpdateDelivery(delivery);
        }
        public void AutoAssignDelivery()
        {
            deliveryDAL.AutoAssignDelivery();
        }
        public Delivery GetDeliveryById(int deliveryId)
        {
            if (deliveryId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(deliveryId), "Delivery ID must be greater than zero");

            }
            return deliveryDAL.GetDeliveryById(deliveryId);
        }
    }   
}
