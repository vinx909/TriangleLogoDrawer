using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.ApplicationCore.Interfaces
{
    public interface IImageService
    {
        public Task<IEnumerable<Image>> GetAll();
        public Task<Image> Get(int imageId);
        public Task<Image> Create(Image image);
        public Task Edit(Image image);
        public Task Remove(int imageId);
        public Task Remove(Image image);
        public Task<IOrderedEnumerable<Order>> GetOrder(Image image, int shapeId);
        public Task<bool> HasOrder(Image image, int shapeId);
    }
}
