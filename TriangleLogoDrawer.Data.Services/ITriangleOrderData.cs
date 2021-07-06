using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services
{
    public interface ITriangleOrderData
    {
        public IEnumerable<TriangleOrder> GetAll();
        public IEnumerable<TriangleOrder> GetAll(int shapeId);
        public List<TriangleOrder> GetOrder(int shapeId);
        public TriangleOrder Get(int shapeId, int triangleId);
        public void Create(TriangleOrder createdTriangleOrder);
        public void Delete(int shapeId, int triangleId);
    }
}
