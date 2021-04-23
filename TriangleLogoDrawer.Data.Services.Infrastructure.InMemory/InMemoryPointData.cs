using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryPointData : IPointData
    {
        private List<Point> points;

        public InMemoryPointData()
        {
            points = new List<Point>()
            {
                new Point(){ Id = 1, ImageId = 1, X = 0, Y = 0},
                new Point(){ Id = 2, ImageId = 1, X = -0.5m, Y = 0},
                new Point(){ Id = 3, ImageId = 1, X = 0, Y = 0.5m},
                new Point(){ Id = 4, ImageId = 1, X = 0.5m, Y = 0},
                new Point(){ Id = 5, ImageId = 1, X = 0, Y = -0.5m},
            };
        }

        public void Create(Point createdPoint)
        {
            points.Add(createdPoint);
        }

        public void Delete(int pointToDeleteId)
        {
            points.Remove(Get(pointToDeleteId));
        }

        public void Edit(Point editedPoint)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Id == editedPoint.Id)
                {
                    points[i] = editedPoint;
                    return;
                }
            }
        }

        public Point Get(int pointId)
        {
            return points.FirstOrDefault(p => p.Id == pointId);
        }

        public IEnumerable<Point> GetAll()
        {
            return points;
        }

        public IEnumerable<Point> GetAll(int imageId)
        {
            return points.Where(p => p.ImageId == imageId);
        }
    }
}
