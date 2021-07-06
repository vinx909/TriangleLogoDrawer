using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace TriangleLogoDrawer.ApplicationCore.Services
{
    public class ShapeService : IShapeService
    {
        private readonly IDataRepository<Shape> shapeRepository;
        private readonly IOrderService orderService;

        public ShapeService(IDataRepository<Shape> shapeRepository, IOrderService orderService)
        {
            this.shapeRepository = shapeRepository;
            this.orderService = orderService;
        }

        public async Task Create(Shape shape)
        {
            await shapeRepository.Create(shape);
        }

        public async Task Edit(Shape shape)
        {
            await shapeRepository.Edit(shape);
        }

        public async Task Remove(int shapeId)
        {
            await Remove(await shapeRepository.Get(shapeId));
        }

        public async Task Remove(Shape shape)
        {
            List<Task> tasks = new();
            foreach (Order order in shape.Orders)
            {
                tasks.Add(orderService.Remove(order));
            }
            await Task.WhenAll(tasks.ToArray());
            await shapeRepository.Remove(shape);
        }
    }
}
