using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace TriangleLogoDrawer.ApplicationCore.Services
{
    public class ImageService:IImageService
    {
        private readonly IDataRepository<Image> imageRepository;
        private readonly IPointService pointService;
        private readonly IShapeService shapeService;

        public ImageService(IDataRepository<Image> imageRepository, IPointService pointService, IShapeService shapeService)
        {
            this.imageRepository = imageRepository;
            this.pointService = pointService;
            this.shapeService = shapeService;
        }

        public async Task<IEnumerable<Image>> GetAll()
        {
            return await imageRepository.GetAll();
        }

        public async Task<Image> Get(int imageId)
        {
            return await imageRepository.Get(imageId);
        }

        public async Task Create(Image image)
        {
            await imageRepository.Create(image);
        }

        public async Task Edit(Image image)
        {
            await imageRepository.Edit(image);
        }

        public async Task Remove(int imageId)
        {
            await Remove(await imageRepository.Get(imageId));
        }
        public async Task Remove(Image image)
        {
            List<Task> tasksList = new();
            foreach(Point point in image.Points)
            {
                tasksList.Add(pointService.Remove(point));
            }
            await Task.WhenAll(tasksList.ToArray());

            tasksList.Clear();
            foreach (Shape shape in image.Shapes)
            {
                tasksList.Add(shapeService.Remove(shape));
            }
            await Task.WhenAll(tasksList.ToArray());

            await imageRepository.Remove(image);
        }

        public async Task<IOrderedEnumerable<Order>> GetOrder(Image image, int shapeId)
        {
            Shape shape = GetShape(image, shapeId);
            if (shape != null)
            {
                return await shapeService.GetOrder(shape);
            }
            else
            {
                return (IOrderedEnumerable<Order>)new List<Order>();
            }
        }
        public async Task<bool> HasOrders(Image image, int shapeId)
        {
            Shape shape = GetShape(image, shapeId);
            if (shape != null)
            {
                return await shapeService.HasOrders(shape);
            }
            else
            {
                return false;
            }
        }

        private Shape GetShape(Image image, int shapeId)
        {
            return image.Shapes.FirstOrDefault(s => s.Id == shapeId);
        }
    }
}
