using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.ApplicationCore.Interfaces
{
    public interface IOrderRepository : IDataRepository<Order>
    {
        public Task<bool> TriangleNotUsed(Triangle triangle);
        public Task<Order> GetOrderWithNumber(Shape shape, int number);
    }
}
