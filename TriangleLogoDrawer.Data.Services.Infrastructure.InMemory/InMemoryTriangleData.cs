using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryTriangleData : TriangleData
    {
        private List<Triangle> triangles;

        public InMemoryTriangleData(ITriangleOrderData triangleOrderData, IShapeData shapeData) : base(triangleOrderData, shapeData)
        {
            triangles = new List<Triangle>()
            {
                new Triangle(){ Id = 1, ImageId = 1, PointIdOne = 1, PointIdTwo = 2, PointIdThree = 3},
                new Triangle(){ Id = 2, ImageId = 1, PointIdOne = 1, PointIdTwo = 4, PointIdThree = 3},
                new Triangle(){ Id = 3, ImageId = 1, PointIdOne = 1, PointIdTwo = 4, PointIdThree = 5},
                new Triangle(){ Id = 4, ImageId = 1, PointIdOne = 1, PointIdTwo = 2, PointIdThree = 5},
                new Triangle(){ Id = 5, ImageId = 1, PointIdOne = 2, PointIdTwo = 3, PointIdThree = 4},
                new Triangle(){ Id = 6, ImageId = 1, PointIdOne = 2, PointIdTwo = 5, PointIdThree = 4},
            };
        }

        protected override void Add(Triangle createdTriangle)
        {
            createdTriangle.Id = triangles.Max(t => t.Id) + 1;
            triangles.Add(createdTriangle);
        }

        public override void Edited(Triangle editedTriangle)
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                if (triangles[i].Id == editedTriangle.Id)
                {
                    triangles[i] = editedTriangle;
                    return;
                }
            }
        }

        public override Triangle Get(int triangleId)
        {
            return triangles.FirstOrDefault(t => t.Id == triangleId);
        }

        public override IEnumerable<Triangle> GetAll()
        {
            return triangles;
        }

        public override IEnumerable<Triangle> GetAll(int imageId)
        {
            return triangles.Where(t => t.ImageId == imageId);
        }

        protected override void Remove(Triangle triangleToDelete)
        {
            triangles.Remove(triangleToDelete);
        }
    }
}
