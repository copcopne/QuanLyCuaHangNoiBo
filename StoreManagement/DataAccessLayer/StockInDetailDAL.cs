using System;
using Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class StockInDetailDAL
    {
        private readonly salesysdbEntities context;
        public StockInDetailDAL(salesysdbEntities context)
        {
            this.context = context;
        }
    }
}
