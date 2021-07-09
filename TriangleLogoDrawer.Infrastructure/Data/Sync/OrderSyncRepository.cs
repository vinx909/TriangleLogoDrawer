using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace TriangleLogoDrawer.Infrastructure.Data.Sync
{
    public class OrderSyncRepository : DataSyncRepository<Order>, IOrderRepository
    {
        public OrderSyncRepository(TriangleDrawerDbContext triangleDrawerDbContext) : base(triangleDrawerDbContext) { }

        public Task<Order> GetOrderWithNumber(Shape shape, int number)
        {
            return Task.FromResult(triangleDrawerDbContext.Set<Order>().FirstOrDefault(o => o.Shape == shape && o.OrderNumber == number));
        }

        public Task<bool> TriangleNotUsed(Triangle triangle)
        {
            if (triangleDrawerDbContext.Set<Order>().FirstOrDefault(o => o.Triangle == triangle) != null)
            {
                return Task.FromResult(false);
            }
            else
            {
                return Task.FromResult(true);
            }
        }
    }
}
