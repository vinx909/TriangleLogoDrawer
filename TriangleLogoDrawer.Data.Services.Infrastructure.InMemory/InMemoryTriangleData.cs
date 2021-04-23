using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryTriangleData : ITriangleData
    {
        private List<Triangle> triangles;

        public InMemoryTriangleData()
        {
            triangles = new List<Triangle>()
            {
                new Triangle(){ Id = 1, ImageId = 1, PointOneId = 1, PointTwoId = 2, PointThreeId = 3},
                new Triangle(){ Id = 2, ImageId = 1, PointOneId = 1, PointTwoId = 4, PointThreeId = 3},
                new Triangle(){ Id = 3, ImageId = 1, PointOneId = 1, PointTwoId = 4, PointThreeId = 5},
                new Triangle(){ Id = 4, ImageId = 1, PointOneId = 1, PointTwoId = 2, PointThreeId = 5},
                new Triangle(){ Id = 5, ImageId = 1, PointOneId = 2, PointTwoId = 3, PointThreeId = 4},
                new Triangle(){ Id = 6, ImageId = 1, PointOneId = 2, PointTwoId = 5, PointThreeId = 4},
            };
        }

        public void Create(Triangle createdTriangle)
        {
            triangles.Add(createdTriangle);
        }

        public void Delete(int triangleToDeleteId)
        {
            triangles.Remove(Get(triangleToDeleteId));
        }

        public void Edited(Triangle editedTriangle)
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

        public Triangle Get(int triangleId)
        {
            return triangles.FirstOrDefault(t => t.Id == triangleId);
        }

        public IEnumerable<Triangle> GetAll()
        {
            return triangles;
        }

        public IEnumerable<Triangle> GetAll(int imageId)
        {
            return triangles.Where(t => t.ImageId == imageId);
        }
    }
}
