using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class StockInBUS
    {
        private readonly DataAccessLayer.StockInDAL stockInDAL;
        private readonly DataAccessLayer.StockInDetailDAL stockInDetailDAL;
        private readonly salesysdbEntities context = new salesysdbEntities();
        public StockInBUS() {
            stockInDAL = new DataAccessLayer.StockInDAL(context);
            stockInDetailDAL = new DataAccessLayer.StockInDetailDAL(context);
        }

        public List<Entity.StockIn> Get(String keyword)
        {
            return stockInDAL.Get(keyword);
        }
        public StockIn Get(int id)
        {
            return stockInDAL.Get(id);
        }

        public List<Entity.StockInDetail> GetStockInDetails(int stockInId)
        {
            return stockInDetailDAL.Get(stockInId);
        }   

        public void AddStockIn(Entity.StockIn stockIn, List<Entity.StockInDetail> stockInDetails)
        {
            if (stockIn == null || stockInDetails == null || !stockInDetails.Any())
            {
                throw new ArgumentException("Dữ liệu nhập hàng không hợp lệ.");
            }
            stockInDAL.Add(stockIn);
            foreach (var d in stockInDetails)
            {
                d.StockInID = stockIn.StockInID;
                d.Product = null;
            }
            stockInDetailDAL.Create(stockIn.StockInID, stockInDetails);
        }

        public void Update(Entity.StockIn stockIn, List<Entity.StockInDetail> stockInDetails)
        {
            if (stockIn == null || stockInDetails == null || !stockInDetails.Any())
            {
                throw new ArgumentException("Dữ liệu nhập hàng không hợp lệ.");
            }
            stockInDetailDAL.Update(stockInDetails);
        }

        public void Delete(List<StockInDetail> details)
        {
            if (details == null || details.Count == 0)
                throw new ArgumentException("Danh sách chi tiết nhập hàng trống.");
            stockInDetailDAL.Delete(details);
        }
        public void Delete(int stockInId)
        {
            if (stockInId <= 0)
                throw new ArgumentException("ID nhập hàng không hợp lệ.");

            stockInDAL.Delete(stockInId);
        }
    }
}
