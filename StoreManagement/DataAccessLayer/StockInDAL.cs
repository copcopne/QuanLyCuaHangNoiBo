using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class StockInDAL
    {
        private readonly salesysdbEntities context;
        public StockInDAL(salesysdbEntities context)
        {
            this.context = context;
        }
        public List<StockIn> GetStockIns()
        {
                return context.StockIns.ToList();
        }
        public void AddStockIn(StockIn stockIn)
        {
            context.StockIns.Add(stockIn);
            context.SaveChanges();
        }
        public void UpdateStockIn(StockIn stockIn)
        {
            var existingStockIn = context.StockIns.FirstOrDefault(s => s.StockInID == stockIn.StockInID);
            if (existingStockIn == null)
            {
                throw new Exception("StockIn not found");
            }
            /// 
            context.SaveChanges();
        }
        public void DeleteStockIn(int id)
        {
            var stockIn = context.StockIns.Find(id);
            if (stockIn != null)
            {
                context.StockIns.Remove(stockIn);
                context.SaveChanges();
            }
        }
    }
}
