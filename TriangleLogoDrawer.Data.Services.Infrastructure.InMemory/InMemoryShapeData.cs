using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryShapeData : ShapeData
    {
        private List<Shape> shapes;

        public InMemoryShapeData(ITriangleOrderData triangleOrderData) : base(triangleOrderData)
        {
            shapes = new List<Shape>()
            {
                new Shape(){ Id = 1, ImageId = 1, Name = "tiny"},
                new Shape(){ Id = 2, ImageId = 1, Name = "bigger"}
            };
        }

        public override void Create(Shape createdShape)
        {
            shapes.Add(createdShape);
        }

        public override void Edit(Shape editedShape)
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

        public override Shape Get(int shapeId)
        {
            return shapes.FirstOrDefault(s => s.Id == shapeId);
        }

        public override IEnumerable<Shape> GetAll()
        {
            return shapes;
        }

        public override IEnumerable<Shape> GetAll(int imageId)
        {
            return shapes.Where(s => s.ImageId == imageId);
        }

        protected override void Remove(int shapeToDeleteId)
        {
            shapes.Remove(Get(shapeToDeleteId));
        }
    }
}
