using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class CategoryDAL
    {
        private readonly salesysdbEntities context;
        public CategoryDAL(salesysdbEntities context)
        {
            this.context = context;
        }
        public List<Category> GetCategories()
        {
            return context.Categories.ToList();
        }
    }
}
