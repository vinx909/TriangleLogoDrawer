using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryShapeData : IShapeData
    {
        private List<Shape> shapes;

        public InMemoryShapeData()
        {
            shapes = new List<Shape>()
            {
                new Shape(){ Id = 1, ImageId = 1, Name = "tiny"},
                new Shape(){ Id = 2, ImageId = 1, Name = "bigger"}
            };
        }

        public void Create(Shape createdShape)
        {
            shapes.Add(createdShape);
        }

        public void Delete(int shapeToDeleteId)
        {
            shapes.Remove(Get(shapeToDeleteId));
        }

        public void Edit(Shape editedShape)
        {
            for (int i = 0; i < shapes.Count; i++)
            {
                if (shapes[i].Id == editedShape.Id)
                {
                    shapes[i] = editedShape;
                    return;
                }
            }
        }

        public Shape Get(int shapeId)
        {
            return shapes.FirstOrDefault(s => s.Id == shapeId);
        }

        public IEnumerable<Shape> GetAll()
        {
            return shapes;
        }

        public IEnumerable<Shape> GetAll(int imageId)
        {
            return shapes.Where(s => s.ImageId == imageId);
        }
    }
}
