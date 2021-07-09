using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.ApplicationCore.Interfaces
{
    public interface IOrderService
    {
        public Task Create(Order order);
        public Task<bool> TriangleNotUsed(Triangle triangle);
        public Task Remove(int orderId);
        public Task Remove(Order order);
        int GetOrderNumber(Triangle workingOnTriangle);
    }
}
