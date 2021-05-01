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
        public IEnumerable<List<TriangleOrder>> GetOrder(int shapeId);
        public List<TriangleOrder> GetOrder(int shapeId, int orderId);
        public List<TriangleOrder> GetOrderFromTriangle(int shapeId, int orderId);
        public TriangleOrder Get(int orderId);
        public void Create(TriangleOrder createdTriangleOrder);
        public void Delete(TriangleOrder orderToDeleteId);
    }
}
