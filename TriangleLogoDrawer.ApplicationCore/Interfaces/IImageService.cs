using System.Collections.Generic;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.ApplicationCore.Interfaces
{
    public interface IImageService
    {
        public Task<IEnumerable<Image>> GetAll();
        public Task<Image> Get(int imageId);
        public Task Create(Image image);
        public Task Edit(Image image);
        public Task Remove(int imageId);
        public Task Remove(Image image);
    }
}
