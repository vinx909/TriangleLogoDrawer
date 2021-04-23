using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.Data;

namespace TriangleLogoDrawer.Data.Services
{
    public interface IImageData
    {
        public IEnumerable<Image> GetAll();
        public Image Get(int imageId);
        public void Create(Image createdImage);
        public void Edit(Image editedImage);
        public void Delete(int imageToDeleteId);
    }
}
