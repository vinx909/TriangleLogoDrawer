using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace TriangleLogoDrawer.ApplicationCore.Services
{
    public class PointService : IPointService
    {
        private readonly IDataRepository<Point> pointRepository;
        private readonly ITriangleService triangleService;

        public PointService(IDataRepository<Point> pointRepository, ITriangleService triangleService)
        {
            this.pointRepository = pointRepository;
            this.triangleService = triangleService;
        }

        public async Task Create(Point point)
        {
            await pointRepository.Create(point);
        }

        public async Task Remove(int pointId)
        {
            await Remove(await pointRepository.Get(pointId));
        }
        public async Task Remove(Point point)
        {
            List<Task> taskList = new();
            foreach (Triangle triangle in point.Triangles)
            {
                taskList.Add(triangleService.Remove(triangle));
            }
            /*
            foreach (Triangle triangle in point.TrianglesPointOne)
            {
                taskList.Add(triangleService.Remove(triangle));
            }
            foreach (Triangle triangle in point.TrianglesPointTwo)
            {
                taskList.Add(triangleService.Remove(triangle));
            }
            foreach (Triangle triangle in point.TrianglesPointThree)
            {
                taskList.Add(triangleService.Remove(triangle));
            }
            */
            await Task.WhenAll(taskList.ToArray());
            await pointRepository.Remove(point);
        }
    }
}
