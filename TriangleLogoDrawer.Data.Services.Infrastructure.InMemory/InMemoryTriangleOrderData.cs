using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryTriangleOrderData : ITriangleOrderData
    {
        private List<TriangleOrder> triangleOrders;

        public InMemoryTriangleOrderData()
        {
            triangleOrders = new List<TriangleOrder>()
            {
                new TriangleOrder(){ ShapeId = 1, TriangleOrigionalId = 1, TriangleFollowingId = 2},
                new TriangleOrder(){ ShapeId = 1, TriangleOrigionalId = 2, TriangleFollowingId = 3},
                new TriangleOrder(){ ShapeId = 1, TriangleOrigionalId = 3, TriangleFollowingId = 4},
                new TriangleOrder(){ ShapeId = 2, TriangleOrigionalId = 5, TriangleFollowingId = 6},
            };
        }

        public void Create(TriangleOrder createdTriangleOrder)
        {
            foreach(TriangleOrder order in GetAll(createdTriangleOrder.ShapeId))
            {
                if (order.TriangleOrigionalId == createdTriangleOrder.TriangleOrigionalId) {
                    {
                        if(order.TriangleFollowingId == createdTriangleOrder.TriangleFollowingId)
                        {
                            return;
                        }
                        order.TriangleOrigionalId = createdTriangleOrder.TriangleFollowingId;
                        break;
                    }
                }
            }
            triangleOrders.Add(createdTriangleOrder);
        }

        public void Delete(int shapeId, int triangleOrigionalId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TriangleOrder> GetAll()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TriangleOrder> GetAll(int shapeId)
        {
            throw new NotImplementedException();
        }

        public List<TriangleOrder> GetOrder(int shapeId, int triangleId)
        {
            throw new NotImplementedException();
        }

        public List<TriangleOrder> GetOrderFromTriangle(int shapeId, int triangleId)
        {
            throw new NotImplementedException();
        }
    }
}
