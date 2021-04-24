using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure
{
    public abstract class PointData : IPointData
    {
        private readonly ITriangleData triangleData;

        protected PointData(ITriangleData triangleData)
        {
            this.triangleData = triangleData;
        }

        public abstract void Create(Point createdPoint);
        public void Delete(int pointToDeleteId)
        {
            Point pointToDelete = Get(pointToDeleteId);
            foreach (Triangle triangle in triangleData.GetAll(pointToDelete.ImageId))
            {
                if(triangle.PointIdOne == pointToDeleteId || triangle.PointIdTwo == pointToDeleteId || triangle.PointIdThree == pointToDeleteId)
                {
                    triangleData.Delete(triangle.Id);
                }
            }
            Remove(pointToDeleteId);
        }
        protected abstract void Remove(int pointToDeleteId);
        public abstract void Edit(Point editedPoint);
        public abstract Point Get(int pointId);
        public abstract IEnumerable<Point> GetAll();
        public abstract IEnumerable<Point> GetAll(int imageId);
    }
}
