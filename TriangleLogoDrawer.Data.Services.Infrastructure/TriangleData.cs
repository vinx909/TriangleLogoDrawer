using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure
{
    public abstract class TriangleData : ITriangleData
    {
        private readonly IShapeData shapeData;
        private readonly ITriangleOrderData triangleOrderData;

        protected TriangleData(ITriangleOrderData triangleOrderData, IShapeData shapeData)
        {
            this.triangleOrderData = triangleOrderData;
            this.shapeData = shapeData;
        }

        public void Create(Triangle createdTriangle)
        {
            foreach(Triangle triangle in GetAll(createdTriangle.ImageId))
            {
                int[] points = new int[3]
                {
                    triangle.PointIdOne,
                    triangle.PointIdTwo,
                    triangle.PointIdThree
                };
                if(points.Contains(createdTriangle.PointIdOne) && points.Contains(createdTriangle.PointIdTwo) && points.Contains(createdTriangle.PointIdThree))
                {
                    return;
                }
            }
            Add(createdTriangle);
        }
        protected abstract void Add(Triangle createdTriangle);
        public void Delete(int triangleToDeleteId)
        {
            Triangle triangleToDelete = Get(triangleToDeleteId);
            List<TriangleOrder> ordersToDetele = new();
            foreach (Shape shape in shapeData.GetAll(triangleToDelete.ImageId))
            {
                foreach (TriangleOrder order in triangleOrderData.GetAll(shape.Id))
                {
                    if(order.TriangleOrigionalId == triangleToDeleteId || order.TriangleFollowingId == triangleToDeleteId)
                    {
                        ordersToDetele.Add(triangleOrderData.Get(order.Id));                                              
                    }
                }
            }
            foreach(TriangleOrder orderToDelete in ordersToDetele)
            {
                triangleOrderData.Delete(orderToDelete);
            }
            Remove(triangleToDelete);
        }
        protected abstract void Remove(Triangle triangleToDelete);
        public abstract void Edited(Triangle editedTriangle);
        public abstract Triangle Get(int triangleId);
        public abstract IEnumerable<Triangle> GetAll();
        public abstract IEnumerable<Triangle> GetAll(int imageId);
    }
}
