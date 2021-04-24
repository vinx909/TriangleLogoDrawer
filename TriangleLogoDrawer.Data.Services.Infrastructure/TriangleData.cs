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

        public abstract void Create(Triangle createdTriangle);
        public void Delete(int triangleToDeleteId)
        {
            Triangle triangleToDelete = Get(triangleToDeleteId);
            foreach (Shape shape in shapeData.GetAll(triangleToDelete.ImageId))
            {
                foreach (TriangleOrder order in triangleOrderData.GetAll(shape.Id))
                {
                    if(order.TriangleOrigionalId == triangleToDeleteId || order.TriangleFollowingId == triangleToDeleteId)
                    {
                        triangleOrderData.Delete(order.Id);
                    }
                }
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
