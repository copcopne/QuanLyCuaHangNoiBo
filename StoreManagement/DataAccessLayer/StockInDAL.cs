using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

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

        public List<StockIn> Get(String keyword)
        {
            var q = context.StockIns
                .AsNoTracking()
                .Include("Employee")
                .Include("StockInDetails.Product");

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var kw = keyword.Trim();

                q = (System.Data.Entity.Infrastructure.DbQuery<StockIn>)q.Where(s =>
                    // Nhân viên: tên hoặc email
                    (s.Employee != null &&
                        (s.Employee.FullName.Contains(kw) ||
                         (s.Employee.Email != null && s.Employee.Email.StartsWith(kw))))
                    ||
                    // Sản phẩm trong phiếu nhập
                    s.StockInDetails.Any(d => d.Product != null &&
                                              d.Product.ProductName.Contains(kw))
                );
            }

            return q.ToList();
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
                .Select(d => (int?)d.UnitCost)
                .Sum() ?? 0;

            existingStockIn.TotalAmount = total;
            context.SaveChanges();
        }

        public void Delete(int stockInID)
        {
            if (stockInID < 0)
            {
                throw new ArgumentException("Danh sách đơn nhập hàng không hợp lệ.");
            }
            var stockIn = context.StockIns.FirstOrDefault(s => s.StockInID == stockInID)
                ?? throw new Exception("Không tìm thấy đơn nhập hàng!");
            StockInDetailDAL stockInDetailDAL = new StockInDetailDAL(context);

            stockInDetailDAL.Delete(stockInID);
            context.StockIns.Remove(stockIn);
            context.SaveChanges();
        }
    }
}
