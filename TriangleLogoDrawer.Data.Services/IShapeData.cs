using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services
{
    public interface IShapeData
    {
        public IEnumerable<Shape> GetAll();
        public IEnumerable<Shape> GetAll(int imageId);
        public Shape Get(int shapeId);
        public void Create(Shape createdShape);
        public void Edit(Shape editedShape);
        public void Delete(int shapeToDeleteId);
    }
}
