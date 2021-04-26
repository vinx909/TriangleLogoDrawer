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
            foreach (Point point in pointData.GetAll(imageToDeleteId))
            {
                pointData.Delete(point.Id);
            }
            foreach(Shape shape in shapeData.GetAll(imageToDeleteId))
            {
                shapeData.Delete(shape.Id);
            }
            Remove(Get(imageToDeleteId));
        }
        protected abstract void Remove(Image imageToDelete);
        public abstract void Edit(Image editedImage);
        public abstract Image Get(int imageId);
        public abstract IEnumerable<Image> GetAll();
    }
}
