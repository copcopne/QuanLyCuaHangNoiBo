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
        private readonly ProductDAL productDAL;
        public StockInDetailDAL()
        {
            this.context = new salesysdbEntities();
            this.stockInDAL = new StockInDAL();
            this.productDAL = new ProductDAL();
        }
        public StockInDetailDAL(salesysdbEntities context)
        {
            this.context = context;
            this.stockInDAL = new StockInDAL(context);
            this.productDAL = new ProductDAL();
        }

        public List<StockInDetail> Get(int stockInId)
        {
            return context.StockInDetails
                .Where(sid => sid.StockInID == stockInId)
                .ToList();
        }

        public void Create(int StockInID, List<StockInDetail> stockInDetails)
        {
            var stockIn = context.StockIns
                .FirstOrDefault(s => s.StockInID == StockInID);
            if (stockIn == null)
            {
                throw new Exception("Không tìm thấy StockInDetail!");
            }
            foreach (var s in stockInDetails)
            {
                context.StockInDetails.Add(s);
                productDAL.UpdateStockQuantity(s.ProductID, s.Quantity);
            }
            ;
            context.SaveChanges();
            stockInDAL.Update(stockIn);
        }

        public void Update(List<StockInDetail> stockInDetails)
        {
            int stockInID = stockInDetails.FirstOrDefault()?.StockInID ?? 0;
            var stockIn = context.StockIns
                .FirstOrDefault(s => s.StockInID == stockInID);
            foreach (var s in stockInDetails)
            {
                var existingDetail = context.StockInDetails
                .FirstOrDefault(sid => sid.StockInID == s.StockInID && sid.ProductID == s.ProductID);
                if (existingDetail != null)
                {
                    int difference = s.Quantity - existingDetail.Quantity;
                    existingDetail.Quantity = s.Quantity;
                    stockInDAL.Update(stockInDAL.Get(existingDetail.StockInID));
                    productDAL.UpdateStockQuantity(existingDetail.ProductID, difference);
                }
                else
                {
                    throw new Exception("Không tìm thấy StockInDetail!");
                }
            }
            stockInDAL.Update(stockIn);
        }

        public void Delete(StockInDetail stockInDetail)
        {
            var s = context.StockInDetails
                .FirstOrDefault(sid => sid.StockInID == stockInDetail.StockInID && sid.ProductID == stockInDetail.ProductID);
            if (stockInDetail != null)
            {
                context.StockInDetails.Remove(s);
                stockInDAL.Update(stockInDAL.Get(stockInDetail.StockInID));
            }
            else
            {
                throw new Exception("Không tìm thấy StockInDetail!");
            }
        }
    }
}
