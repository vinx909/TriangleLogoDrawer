using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryImageData : ImageData
    {
        private readonly List<Image> images;

        public InMemoryImageData(IPointData pointData, IShapeData shapeData) : base(pointData, shapeData)
        {
            images = new List<Image> {
                new Image() { Id = 1, Name = "test 1", BackgroundImagePath = "C:\\Users\\vinx.909\\Pictures\\channel logo by BadAnimations101.jpg" },
                new Image() { Id = 2, Name = "test 2" },
                new Image() { Id = 3, Name = "test 3" }
            };
        }

        public override int Create(Image createdImage)
        {
            createdImage.Id = images.Max(i => i.Id) + 1;
            images.Add(createdImage);
            return createdImage.Id;
        }

        public override void Edit(Image editedImage)
        {
            for (int i = 0; i < images.Count; i++)
            {
                if(images[i].Id == editedImage.Id)
                {
                    images[i] = editedImage;
                    return;
                }
            }
        }

        public override Image Get(int imageId)
        {
            return images.FirstOrDefault(i => i.Id == imageId);
        }

        public override IEnumerable<Image> GetAll()
        {
            return images;
        }

        protected override void Remove(Image imageToDelete)
        {
            images.Remove(imageToDelete);
        }
    }
}
