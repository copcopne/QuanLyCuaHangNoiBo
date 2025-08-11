using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLayer
{
    public class CategoryBUS
    {
        private readonly salesysdbEntities context;
        private readonly CategoryDAL categoryDAL;
        public CategoryBUS(salesysdbEntities context)
        {
            this.categoryDAL = new CategoryDAL(context);
        }
        public List<Category> GetCategories()
        {
            return categoryDAL.GetCategories();
        }
    }
}
