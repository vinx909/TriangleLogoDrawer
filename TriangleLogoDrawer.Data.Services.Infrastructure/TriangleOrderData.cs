using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure
{
    public abstract class TriangleOrderData : ITriangleOrderData
    {
        public abstract IEnumerable<TriangleOrder> GetAll();
        public abstract IEnumerable<TriangleOrder> GetAll(int shapeId);
        public List<TriangleOrder> GetOrder(int shapeId)
        {
            foreach(TriangleOrder thereAreTrianglesToOrder in GetAll(shapeId))
            {
                int index = 0;
                while (true)
                {
                    TriangleOrder firstOrder = GetFromOrderNumber(shapeId, index);
                    if(firstOrder != null)
                    {
                        return GetNextOrder(new List<TriangleOrder> { firstOrder });
                    }
                    index++;
                }
            }
            return null;
        }
        public abstract TriangleOrder Get(int shapeId, int triangleId);
        public void Create(TriangleOrder createdTriangleOrder)
        {
            if(Get(createdTriangleOrder.ShapeId, createdTriangleOrder.TriangleId) == null)
            {
                TryUpOrder(createdTriangleOrder.ShapeId, createdTriangleOrder.OrderNumber);
                Add(createdTriangleOrder);
            }
        }
        public void Delete(int shapeId, int triangleId)
        {
            TriangleOrder toDelete = Get(shapeId, triangleId);
            Remove(toDelete);
            TryLowerOrder(shapeId, toDelete.OrderNumber + 1);
        }

        protected abstract TriangleOrder GetFromOrderNumber(int shapeId, int orderNumber);
        protected abstract void Add(TriangleOrder addedTriangleOrder);
        protected abstract void Edit(TriangleOrder editedTrianlgeOrder);
        protected abstract void Remove(TriangleOrder orderToDelete);

        private void TryUpOrder(int shapeId, int orderNumber)
        {
            TriangleOrder order = GetFromOrderNumber(shapeId, orderNumber);
            if(order != null)
            {
                TryUpOrder(shapeId, orderNumber + 1);
                order.OrderNumber += 1;
                Edit(order);
            }
        }
        private void TryLowerOrder(int shapeId, int orderNumber)
        {
            TriangleOrder order = GetFromOrderNumber(shapeId, orderNumber);
            if (order != null)
            {
                order.OrderNumber -= 1;
                Edit(order);
                TryLowerOrder(shapeId, orderNumber + 1);
            }
        }
        private List<TriangleOrder> GetNextOrder(List<TriangleOrder> existingOrder)
        {
            TriangleOrder lastOrder = existingOrder.Last();
            TriangleOrder newOrderItem = GetFromOrderNumber(lastOrder.ShapeId, lastOrder.OrderNumber + 1);
            if(newOrderItem != null)
            {
                existingOrder.Add(newOrderItem);
                return GetNextOrder(existingOrder);
            }
            else
            {
                return existingOrder;
            }
        }
    }
}
