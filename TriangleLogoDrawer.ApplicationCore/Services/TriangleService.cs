using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace TriangleLogoDrawer.ApplicationCore.Services
{
    public class TriangleService : ITriangleService
    {
        private readonly IDataRepository<Triangle> triangleRepository;
        private readonly IOrderService orderService;

        public TriangleService(IDataRepository<Triangle> triangleRepository, IOrderService orderService)
        {
            this.triangleRepository = triangleRepository;
            this.orderService = orderService;
        }

        public async Task Create(Triangle triangle)
        {
            await triangleRepository.Create(triangle);
        }

        public async Task Remove(int triangleId)
        {
            await Remove(await triangleRepository.Get(triangleId));
        }

        public async Task Remove(Triangle triangle)
        {
            if(triangle.order != null)
            {
                await orderService.Remove(triangle.order);
            }
            await triangleRepository.Remove(triangle);
        }
    }
}
