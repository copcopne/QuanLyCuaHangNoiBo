using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DataAccessLayer
{
    public class StockRequestDAL
    {
        private readonly salesysdbEntities context;
        public StockRequestDAL(salesysdbEntities context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void Add(Entity.StockRequest stockRequest)
        {
            if (stockRequest == null)
            {
                throw new ArgumentNullException("StockRequest không được null!");
            }
            stockRequest.RequestDate = DateTime.Now;
            stockRequest.Status = "Chờ duyệt";
            context.StockRequests.Add(stockRequest);
            context.SaveChanges();
        }
        public List<Entity.StockRequest> GetAll()
        {
            return context.StockRequests.ToList();
        }
        public Entity.StockRequest GetById(int id)
        {
            return context.StockRequests.Find(id);
        }

    }
}
