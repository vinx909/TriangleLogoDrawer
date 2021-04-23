using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services
{
    public interface ITriangleData
    {
        public IEnumerable<Triangle> GetAll();
        public IEnumerable<Triangle> GetAll(int imageId);
        public Triangle Get(int triangleId);
        public void Create(Triangle createdTriangle);
        public void Edited(Triangle editedTriangle);
        public void Delete(int triangleToDeleteId);
    }
}
