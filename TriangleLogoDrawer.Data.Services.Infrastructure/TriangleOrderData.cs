using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure
{
    public abstract class TriangleOrderData : ITriangleOrderData
    {
        public void Create(TriangleOrder createdTriangleOrder)
        {
            foreach (TriangleOrder order in GetAll(createdTriangleOrder.ShapeId))
            {
                if (order.TriangleOrigionalId == createdTriangleOrder.TriangleOrigionalId)
                {
                    {
                        if (order.TriangleFollowingId == createdTriangleOrder.TriangleFollowingId)
                        {
                            return;
                        }
                        order.TriangleOrigionalId = createdTriangleOrder.TriangleFollowingId;
                        Edit(order);
                        break;
                    }
                }
            }
            Add(createdTriangleOrder);
        }
        protected abstract void Add(TriangleOrder addedTriangleOrder);
        protected abstract void Edit(TriangleOrder editedTrianlgeOrder);
        public void Delete(int orderToDeleteId)
        {
            TriangleOrder toDelete = Get(orderToDeleteId);
            Delete(toDelete);
        }
        public void Delete(TriangleOrder orderToDelete)
        {
            foreach (TriangleOrder orderEndsWithToDeleteStart in GetAll(orderToDelete.ShapeId))
            {
                if(orderEndsWithToDeleteStart.TriangleFollowingId == orderToDelete.TriangleOrigionalId)
                {
                    foreach (TriangleOrder orderStartsWithToDeleteEnd in GetAll(orderToDelete.ShapeId))
                    {
                        if (orderStartsWithToDeleteEnd.TriangleOrigionalId == orderToDelete.TriangleFollowingId)
                        {
                            orderEndsWithToDeleteStart.TriangleFollowingId = orderStartsWithToDeleteEnd.TriangleFollowingId;
                            Edit(orderEndsWithToDeleteStart);
                            Remove(orderToDelete);
                            Delete(orderStartsWithToDeleteEnd);
                            return;
                        }
                    }
                }
            }
            Remove(orderToDelete);
        }
        protected abstract void Remove(TriangleOrder orderToDelete);
        public abstract IEnumerable<TriangleOrder> GetAll();
        public abstract IEnumerable<TriangleOrder> GetAll(int shapeId);
        public IEnumerable<List<TriangleOrder>> GetOrder(int shapeId)
        {
            List<TriangleOrder> allOrders = new List<TriangleOrder>(GetAll(shapeId));
            List<List<TriangleOrder>> toReturn = new List<List<TriangleOrder>>();
            while(allOrders.Count() != 0)
            {
                List<TriangleOrder> newOrder = GetOrder(shapeId, allOrders.FirstOrDefault().Id);
                toReturn.Add(newOrder);
                foreach (TriangleOrder newOrderpart in newOrder)
                {
                    allOrders.RemoveAll(o => o.Id == newOrderpart.Id);
                }
            }
            return toReturn;
        }
        public List<TriangleOrder> GetOrder(int shapeId, int orderId)
        {
            return GetOrder(shapeId, orderId, GetOrderFromTriangle(shapeId, orderId));
        }
        private List<TriangleOrder> GetOrder(int shapeId, int orderId, List<TriangleOrder> existingOrder)
        {
            foreach (TriangleOrder order in GetAll(shapeId))
            {
                if (order.TriangleFollowingId == orderId)
                {
                    existingOrder.Insert(0, order);
                    return GetOrder(shapeId, order.Id, existingOrder);
                }
            }
            return existingOrder;
        }
        public List<TriangleOrder> GetOrderFromTriangle(int shapeId, int orderId)
        {
            List<TriangleOrder> order = new List<TriangleOrder>();
            TriangleOrder previousOrder = Get(orderId);
            order.Add(previousOrder);
            bool addition;
            do
            {
                addition = false;
                foreach (TriangleOrder triangleOrder in GetAll(shapeId))
                {
                    if (previousOrder.TriangleFollowingId == triangleOrder.TriangleOrigionalId)
                    {
                        order.Add(triangleOrder);
                        previousOrder = triangleOrder;
                        addition = true;
                        break;
                    }
                }
            }
            while (addition == true);
            return order;
        }
        public abstract TriangleOrder Get(int orderId);
    }
}
