using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace TriangleLogoDrawer.Infrastructure.Data
{
    public class OrderRepository : DataRepository<Order>, IOrderRepository
    {
        public OrderRepository(TriangleDrawerDbContext triangleDrawerDbContext) : base(triangleDrawerDbContext) { }

        public async Task<Order> GetOrderWithNumber(Shape shape, int number)
        {
            return await triangleDrawerDbContext.Set<Order>().FirstOrDefaultAsync(o => o.Shape == shape && o.OrderNumber == number);
        }

        public async Task<bool> TriangleNotUsed(Triangle triangle)
        {
            if (await triangleDrawerDbContext.Set<Order>().FirstOrDefaultAsync(o => o.Triangle == triangle) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
