using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataObjects.AdoNet
{
    // Data access object factory
    // ** Factory Pattern

    public class DaoFactory : IDaoFactory
    {
        public IMemberDao MemberDao => new MemberDao();
        public IOrderDao OrderDao => new OrderDao();
        public IOrderDetailDao OrderDetailDao => new OrderDetailDao();
        public IProductDao ProductDao => new ProductDao();
        public ICategoryDao CategoryDao => new CategoryDao();
    }
}
