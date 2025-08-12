using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace BusinessLayer
{
    public class StockRequestBUS
    {
        private readonly DataAccessLayer.StockRequestDAL stockRequestDAL;
        private readonly DataAccessLayer.StockRequestDetailDAL stockRequestDetailDAL;
        private readonly salesysdbEntities context = new salesysdbEntities();
        public StockRequestBUS() {
            stockRequestDAL = new DataAccessLayer.StockRequestDAL(context);
            stockRequestDetailDAL = new DataAccessLayer.StockRequestDetailDAL(context);
        }

        public void AddStockRequest(Entity.StockRequest stockRequest, List<Entity.StockRequestDetail> stockRequestDetails)
        {
            if (stockRequest == null || stockRequestDetails == null || !stockRequestDetails.Any())
            {
                throw new ArgumentException("Dữ liệu yêu cầu nhập hàng không hợp lệ.");
            }
            stockRequestDAL.Add(stockRequest);
            foreach (var d in stockRequestDetails)
            {
                d.RequestID = stockRequest.RequestID;
                d.Product = null; // Avoid circular reference
            }
            stockRequestDetailDAL.Create(stockRequest.RequestID, stockRequestDetails);
        }
    }
}
