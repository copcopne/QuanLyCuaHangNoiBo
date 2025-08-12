using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class StockInDAL
    {
        private readonly salesysdbEntities context;
        public StockInDAL()
        {
            this.context = new salesysdbEntities();
        }
        public StockInDAL(salesysdbEntities context)
        {
            this.context = context;
        }

        public List<StockIn> Get()
        {
            return context.StockIns.AsNoTracking().ToList();
        }
        public StockIn Get(int stockInId)
        {
            return context.StockIns.AsNoTracking().FirstOrDefault(s => s.StockInID == stockInId)
                ?? throw new Exception("Không tìm thấy đơn nhập hàng!");
        }

        public void Add(StockIn stockIn)
        {
            stockIn.StockInDate = DateTime.Now;
            context.StockIns.Add(stockIn);
            context.SaveChanges();
        }
        public void Update(StockIn stockIn)
        {
            var existingStockIn = context.StockIns.FirstOrDefault(s => s.StockInID == stockIn.StockInID)
                ?? throw new Exception("Không tìm thấy đơn nhập hàng!");

            int total = context.StockInDetails
                .Where(d => d.StockInID == stockIn.StockInID)
                .Select(d => (int?)d.Quantity)
                .Sum() ?? 0;

            existingStockIn.TotalAmount = total;
            context.SaveChanges();
        }
    }
}
