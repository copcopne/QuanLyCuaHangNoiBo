using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class ProductDAL
    {
        private readonly salesysdbEntities context;

        public ProductDAL(salesysdbEntities context)
        {
            this.context = context;
        }

        public List<Product> GetProducts(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return context.Products.ToList();
            }
            else
            {
                return context.Products
                    .Where(p => p.ProductName.Contains(keyword))
                    .ToList();
            }
        }

        public Product GetProductById(int productId)
        {
            return context.Products.FirstOrDefault(p => p.ProductID == productId);
        }


        public void AddProduct(Product product)
        {
            context.Products.Add(product);
            context.SaveChanges();
        }

        public void UpdateProduct(Product product)
        {
            var existingProduct = context.Products.FirstOrDefault(p => p.ProductID == product.ProductID);
            if (existingProduct != null)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.CategoryID = product.CategoryID;
                existingProduct.StockQuantity = product.StockQuantity;
                existingProduct.MinStockLevel = product.MinStockLevel;
                existingProduct.Unit = product.Unit;

                context.SaveChanges();
            }
        }

        public void DeleteProduct(int productId)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductID == productId);
            if (product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }
    }
}
