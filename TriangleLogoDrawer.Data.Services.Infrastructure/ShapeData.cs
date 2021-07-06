using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure
{
    public abstract class ShapeData : IShapeData
    {
        private readonly ITriangleOrderData triangleOrderData;

        protected ShapeData(ITriangleOrderData triangleOrderData)
        {
            this.triangleOrderData = triangleOrderData;
        }

        public abstract void Create(Shape createdShape);
        public void Delete(int shapeToDeleteId)
        {
            List<int> orderTriangleIdsToDelete = new();
            foreach(TriangleOrder order in triangleOrderData.GetAll(shapeToDeleteId))
            {
                orderTriangleIdsToDelete.Add(order.TriangleId);
            }
            foreach(int orderTriangleIdToDelete in orderTriangleIdsToDelete)
            {
                triangleOrderData.Delete(shapeToDeleteId, orderTriangleIdToDelete);
            }
            Remove(shapeToDeleteId);
        }
        protected abstract void Remove(int shapeToDeleteId);
        public abstract void Edit(Shape editedShape);
        public abstract Shape Get(int shapeId);
        public abstract IEnumerable<Shape> GetAll();
        public abstract IEnumerable<Shape> GetAll(int imageId);
    }
}
