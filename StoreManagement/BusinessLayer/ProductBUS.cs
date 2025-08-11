using System;
using Entity;
using DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class ProductBUS
    {
        private readonly salesysdbEntities context;
        private readonly ProductDAL productDAL;
        public ProductBUS(salesysdbEntities context)
        {
            this.context = context;
            this.productDAL = new ProductDAL(context);
        }
        public List<Product> GetProducts(string keyword)
        {
            return productDAL.GetProducts(keyword);
        }
        public Product GetProductById(int productId)
        {
            return productDAL.GetProductById(productId);
        }
        public List<Product> GetProductsByCategory(int categoryId)
        {
            return productDAL.GetProductsByCategory(categoryId);
        }
        public void AddProduct(Product product)
        {
            productDAL.AddProduct(product);
        }
        public void UpdateProduct(Product product)
        {
            productDAL.UpdateProduct(product);
        }
        public void DeleteProduct(int productId)
        {
            productDAL.DeleteProduct(productId);
        }
        public void updateProductQuantity(int productId, int quantity)
        {
            productDAL.updateStockQuantity(productId, quantity);
        }
    }
}
