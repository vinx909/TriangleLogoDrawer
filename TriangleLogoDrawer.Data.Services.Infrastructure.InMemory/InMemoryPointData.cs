using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryPointData : PointData
    {
        private List<Point> points;

        public InMemoryPointData(ITriangleData triangleData) : base(triangleData)
        {
            points = new List<Point>()
            {
                new Point(){ Id = 1, ImageId = 1, X = 0.1m, Y = 0},
                new Point(){ Id = 2, ImageId = 1, X = -0.5m, Y = 0},
                new Point(){ Id = 3, ImageId = 1, X = 0, Y = 0.5m},
                new Point(){ Id = 4, ImageId = 1, X = 0.5m, Y = 0},
                new Point(){ Id = 5, ImageId = 1, X = 0, Y = -0.5m},
            };
        }

        public override void Create(Point createdPoint)
        {
            createdPoint.Id = points.Max(p => p.Id) + 1;
            points.Add(createdPoint);
        }

        public override void Edit(Point editedPoint)
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

        public override Point Get(int pointId)
        {
            return points.FirstOrDefault(p => p.Id == pointId);
        }

        public override IEnumerable<Point> GetAll()
        {
            return points;
        }

        public override IEnumerable<Point> GetAll(int imageId)
        {
            return points.Where(p => p.ImageId == imageId);
        }

        protected override void Remove(int pointToDeleteId)
        {
            points.Remove(Get(pointToDeleteId));
        }
    }
}
