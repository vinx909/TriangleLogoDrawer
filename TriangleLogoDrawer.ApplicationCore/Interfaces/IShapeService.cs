using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.ApplicationCore.Interfaces
{
    public interface IShapeService
    {
        public Task Create(Shape shape);
        public Task Edit(Shape shape);
        public Task Remove(int shapeId);
        public Task Remove(Shape shape);
        public Task<IOrderedEnumerable<Order>> GetOrder(Shape shape);
        public Task<bool> HasOrders(Shape shape);
    }
}
