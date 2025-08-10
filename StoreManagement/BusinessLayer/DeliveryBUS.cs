using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class DeliveryBUS
    {
        private readonly DataAccessLayer.DeliveryDAL deliveryDAL;
        public DeliveryBUS(salesysdbEntities context)
        {
            this.deliveryDAL = new DataAccessLayer.DeliveryDAL(context);
        }
        public List<Entity.Delivery> GetDeliveries()
        {
            return deliveryDAL.GetDeliveries();
        }
        public void AddDelivery(Entity.Delivery delivery)
        {
            if (delivery == null)
            {
                throw new ArgumentNullException(nameof(delivery), "Delivery cannot be null");
            }
            deliveryDAL.AddDelivery(delivery);
        }
    }
}
