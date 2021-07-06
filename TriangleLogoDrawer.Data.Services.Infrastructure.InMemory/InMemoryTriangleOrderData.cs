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
                new TriangleOrder(){ ShapeId = 1, TriangleId = 1, OrderNumber = 0},
                new TriangleOrder(){ ShapeId = 1, TriangleId = 2, OrderNumber = 1},
                new TriangleOrder(){ ShapeId = 1, TriangleId = 3, OrderNumber = 2},
                new TriangleOrder(){ ShapeId = 1, TriangleId = 4, OrderNumber = 3},
                new TriangleOrder(){ ShapeId = 2, TriangleId = 5, OrderNumber = 0},
                new TriangleOrder(){ ShapeId = 2, TriangleId = 6, OrderNumber = 1}
            };
        }

        public override TriangleOrder Get(int shapeId, int triangleId)
        {
            return triangleOrders.FirstOrDefault(t => t.ShapeId == shapeId && t.TriangleId == triangleId);
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
            Get(editedTrianlgeOrder.ShapeId, editedTrianlgeOrder.TriangleId).OrderNumber = editedTrianlgeOrder.OrderNumber;
        }
        protected override TriangleOrder GetFromOrderNumber(int shapeId, int orderNumber)
        {
            return triangleOrders.FirstOrDefault(t => t.ShapeId == shapeId && t.OrderNumber == orderNumber);
        }
        protected override void Remove(TriangleOrder orderToDelete)
        {
            triangleOrders.Remove(Get(orderToDelete.ShapeId, orderToDelete.TriangleId));
        }
    }
}
