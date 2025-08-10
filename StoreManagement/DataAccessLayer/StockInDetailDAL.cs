using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class StockInDetailDAL
    {
        private readonly salesysdbEntities context;
        private readonly StockInDAL stockInDAL;
        public StockInDetailDAL()
        {
            this.context = new salesysdbEntities();
            this.stockInDAL = new StockInDAL();
        }
        public StockInDetailDAL(salesysdbEntities context)
        {
            this.context = context;
            this.stockInDAL = new StockInDAL(context);
        }

        public List<StockInDetail> Get(int stockInId)
        {
            return context.StockInDetails
                .Where(sid => sid.StockInID == stockInId)
                .ToList();
        }

        public StockInDetail Create(StockInDetail stockInDetail)
        {
            if (stockInDetail == null)
            {
                throw new Exception("Không tìm thấy StockInDetail!");
            }
            context.StockInDetails.Add(stockInDetail);
            context.SaveChanges();
            return stockInDetail;
        }

        public StockInDetail Update(StockInDetail stockInDetail)
        {
            if (stockInDetail == null)
            {
                throw new Exception("Không tìm thấy StockInDetail!");
            }
            var existingDetail = context.StockInDetails
                .FirstOrDefault(sid => sid.StockInID == stockInDetail.StockInID && sid.ProductID == stockInDetail.ProductID);
            if (existingDetail != null)
            {
                existingDetail.Quantity = stockInDetail.Quantity;
                existingDetail.UnitCost = stockInDetail.UnitCost;
                stockInDAL.Update(stockInDAL.Get(existingDetail.StockInID));
                return existingDetail;
            }
            else
            {
                throw new Exception("Không tìm thấy StockInDetail!");
            }
        }

        public void Delete(StockInDetail stockInDetail)
        {
            var s = context.StockInDetails
                .FirstOrDefault(sid => sid.StockInID == stockInDetail.StockInID && sid.ProductID == stockInDetail.ProductID);
            if (stockInDetail != null)
            {
                context.StockInDetails.Remove(s);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Không tìm thấy StockInDetail!");
            }
        }
    }
}
