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
                .FirstOrDefault(s => s.StockInID == StockInID) ?? throw new Exception("Không tìm thấy StockInDetail!");
            foreach (var s in stockInDetails)
            {
                context.StockInDetails.Add(s);
                productDAL.UpdateStockQuantity(s.ProductID, s.Quantity);
            }
            ;
            context.SaveChanges();
            stockInDAL.Update(stockIn);
        }

        public void Create(StockIn stockIn, StockInDetail stockInDetail)
        {
            context.StockInDetails.Add(stockInDetail);
            productDAL.UpdateStockQuantity(stockInDetail.ProductID, stockInDetail.Quantity);

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
                // Cập nhật số lượng sản phẩm đã có
                if (existingDetail != null)
                {
                    int difference = s.Quantity - existingDetail.Quantity;
                    existingDetail.Quantity = s.Quantity;
                    stockInDAL.Update(stockInDAL.Get(existingDetail.StockInID));
                    productDAL.UpdateStockQuantity(existingDetail.ProductID, difference);
                }
                // Cập nhật sản phẩm mới
                else
                {
                    StockInDetail newDetail = new StockInDetail
                    {
                        StockInID = stockInID,
                        ProductID = s.ProductID,
                        Quantity = s.Quantity,
                        UnitCost = s.UnitCost,
                    };
                    this.Create(stockIn, newDetail);
                }
            }
            stockInDAL.Update(stockIn);
        }

        public void Delete(List<StockInDetail> stockInDetails)
        {
            foreach (var stockInDetail in stockInDetails)
            {
                var s = context.StockInDetails
                    .FirstOrDefault(sid => sid.StockInID == stockInDetail.StockInID
                                        && sid.ProductID == stockInDetail.ProductID);

                if (s != null)
                {
                    context.StockInDetails.Remove(s);
                    productDAL.UpdateStockQuantity(s.ProductID, -s.Quantity);
                    var stockIn = stockInDAL.Get(stockInDetail.StockInID);
                    if (stockIn != null)
                    {
                        stockInDAL.Update(stockIn);
                    }
                }
                else
                {
                    throw new Exception($"Không tìm thấy StockInDetail cho ProductID={stockInDetail.ProductID}, StockInID={stockInDetail.StockInID}!");
                }
            }

            context.SaveChanges();
        }

        public void Delete(int stockInId)
        {
            var stockInDetails = context.StockInDetails
                .Where(sid => sid.StockInID == stockInId)
                .ToList();
            if (stockInDetails.Count == 0)
            {
                throw new Exception("Không tìm thấy StockInDetail nào để xóa!");
            }
            foreach (var detail in stockInDetails)
            {
                context.StockInDetails.Remove(detail);
                productDAL.UpdateStockQuantity(detail.ProductID, -detail.Quantity);
            }
            context.SaveChanges();
        }
    }
}
