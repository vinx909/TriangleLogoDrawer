using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure
{
    public abstract class ImageData : IImageData
    {
        private readonly IPointData pointData;
        private readonly IShapeData shapeData;

        protected ImageData(IPointData pointData, IShapeData shapeData)
        {
            this.pointData = pointData;
            this.shapeData = shapeData;
        }

        public abstract int Create(Image createdImage);
        public void Delete(int imageToDeleteId)
        {
            List<int> pointIdsToDelete = new();
            foreach (Point point in pointData.GetAll(imageToDeleteId))
            {
                pointIdsToDelete.Add(point.Id);
            }
            foreach(int idToDelete in pointIdsToDelete)
            {
                pointData.Delete(idToDelete);
            }

            List<int> shapeIdsToDelete = new();
            foreach(Shape shape in shapeData.GetAll(imageToDeleteId))
            {
                shapeIdsToDelete.Add(shape.Id);
            }
            foreach(int idToDelete in shapeIdsToDelete)
            {
                shapeData.Delete(idToDelete);
            }

            Remove(Get(imageToDeleteId));
        }
        protected abstract void Remove(Image imageToDelete);
        public abstract void Edit(Image editedImage);
        public abstract Image Get(int imageId);
        public abstract IEnumerable<Image> GetAll();
    }
}
