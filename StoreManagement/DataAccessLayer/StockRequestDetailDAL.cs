using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
namespace DataAccessLayer
{
    public class StockRequestDetailDAL
    {
        private readonly salesysdbEntities context;
        public StockRequestDetailDAL()
        {
            context = new salesysdbEntities();
        }
        public StockRequestDetailDAL(salesysdbEntities context)
        {
            this.context = context;
        }
        //public List<StockRequestDetail> Get(int stockRequestId)
        //{
        //    return context.StockRequestDetails
        //        .Where(srd => srd.RequestID == stockRequestId)
        //        .ToList();
        //}
        public void Create(int stockRequestId, List<StockRequestDetail> stockRequestDetails)
        {
            var stockRequest = context.StockRequests
                .FirstOrDefault(sr => sr.RequestID == stockRequestId) 
                ?? throw new Exception("Không tìm thấy StockRequestDetail!");
            foreach (var srd in stockRequestDetails)
            {
                context.StockRequestDetails.Add(srd);
            }
            context.SaveChanges();
        }
    }
}
