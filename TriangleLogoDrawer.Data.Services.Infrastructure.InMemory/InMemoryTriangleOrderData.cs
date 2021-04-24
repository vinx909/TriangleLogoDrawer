using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryTriangleOrderData : TriangleOrderData
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

        public override TriangleOrder Get(int orderId)
        {
            return triangleOrders.FirstOrDefault(t => t.Id == orderId);
        }
        public override IEnumerable<TriangleOrder> GetAll()
        {
            return triangleOrders;
        }
        public override IEnumerable<TriangleOrder> GetAll(int shapeId)
        {
            return triangleOrders.Where(t => t.ShapeId == shapeId);
        }

        protected override void Add(TriangleOrder addedTriangleOrder)
        {
            triangleOrders.Add(addedTriangleOrder);
        }

        protected override void Edit(TriangleOrder editedTrianlgeOrder)
        {
            for (int i = 0; i < triangleOrders.Count; i++)
            {
                if (triangleOrders[i].Id == editedTrianlgeOrder.Id)
                {
                    triangleOrders[i] = editedTrianlgeOrder;
                    return;
                }
            }
        }

        protected override void Remove(TriangleOrder orderToDelete)
        {
            triangleOrders.Remove(orderToDelete);
        }
    }
}
