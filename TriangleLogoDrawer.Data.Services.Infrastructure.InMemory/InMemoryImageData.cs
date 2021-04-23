using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.Infrastructure.InMemory
{
    public class InMemoryImageData : IImageData
    {
        private readonly List<Image> images;

        public InMemoryImageData()
        {
            images = new List<Image> {
                new Image() { Id = 1, Name = "test 1" },
                new Image() { Id = 2, Name = "test 2" },
                new Image() { Id = 3, Name = "test 3" }
            };
        }

        public void Create(Image createdImage)
        {
            images.Add(createdImage);
        }

        public void Delete(int imageToDeleteId)
        {
            images.Remove(Get(imageToDeleteId));
        }

        public void Edit(Image editedImage)
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

        public Image Get(int imageId)
        {
            return images.FirstOrDefault(i => i.Id == imageId);
        }

        public IEnumerable<Image> GetAll()
        {
            return images;
        }
    }
}
