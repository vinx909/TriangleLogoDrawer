using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services
{
    public interface IPointData
    {
        public IEnumerable<Point> GetAll();
        public IEnumerable<Point> GetAll(int imageId);
        public Point Get(int pointId);
        public void Create(Point createdPoint);
        public void Edit(Point editedPoint);
        public void Delete(int pointToDeleteId);
    }
}
