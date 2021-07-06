using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Interfaces;
using TriangleLogoDrawer.ApplicationCore.Services;
using TriangleLogoDrawer.Data.Services.Infrastructure.InMemory;
using TriangleLogoDrawer.Infrastructure.Data;
using TriangleLogoDrawer.SimpleDependencyProvider;

namespace TriangleLogoDrawer.Data.Services.SimpleDependencyProvidedSetup
{
    public static class SimpleDependencyProviderSetup
    {
        private enum options
        {
            InMemory,
            ApplicationCoreAndInfrastructure
        }
        private const options option = options.ApplicationCoreAndInfrastructure;

        public static void Setup()
        {
            switch (option)
            {
                case options.InMemory:

                    InMemorySingleInstanceContainer container = new InMemorySingleInstanceContainer();
                    container.InMemoryTriangleOrderData = new InMemoryTriangleOrderData();
                    container.InMemoryShapeData = new InMemoryShapeData(container.InMemoryTriangleOrderData);
                    container.InMemoryTriangleData = new InMemoryTriangleData(container.InMemoryTriangleOrderData, container.InMemoryShapeData);
                    container.InMemoryPointData = new InMemoryPointData(container.InMemoryTriangleData);
                    container.InMemoryImageData = new InMemoryImageData(container.InMemoryPointData, container.InMemoryShapeData);

                    Func<IImageData> imageProvider = () => { return container.InMemoryImageData; };
                    DependencyProvider.Add(typeof(IImageData), imageProvider);

                    Func<IPointData> pointProvider = () => { return container.InMemoryPointData; };
                    DependencyProvider.Add(typeof(IPointData), pointProvider);

                    Func<IShapeData> shapeProvider = () => { return container.InMemoryShapeData; };
                    DependencyProvider.Add(typeof(IShapeData), shapeProvider);

                    Func<ITriangleData> triangleProvider = () => { return container.InMemoryTriangleData; };
                    DependencyProvider.Add(typeof(ITriangleData), triangleProvider);

                    Func<ITriangleOrderData> orderProvider = () => { return container.InMemoryTriangleOrderData; };
                    DependencyProvider.Add(typeof(ITriangleOrderData), orderProvider);
                    break;

                case options.ApplicationCoreAndInfrastructure:
                    DependencyProvider.Add(typeof(TriangleDrawerDbContext), () => { return new TriangleDrawerDbContext(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TriangleDrawer;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"); });

                    DependencyProvider.Add(typeof(IDataRepository<ApplicationCore.Entities.Image>), () => { return new DataRepository<Image>(DependencyProvider.Provide<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add(typeof(IDataRepository<ApplicationCore.Entities.Point>), () => { return new DataRepository<Point>(DependencyProvider.Provide<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add(typeof(IDataRepository<ApplicationCore.Entities.Shape>), () => { return new DataRepository<Shape>(DependencyProvider.Provide<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add(typeof(IDataRepository<ApplicationCore.Entities.Triangle>), () => { return new DataRepository<Triangle>(DependencyProvider.Provide<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add(typeof(IOrderRepository), () => { return new OrderRepository(DependencyProvider.Provide<TriangleDrawerDbContext>()); });

                    DependencyProvider.Add(typeof(IImageService), () => { return new ImageService(DependencyProvider.Provide<IDataRepository<ApplicationCore.Entities.Image>>(), DependencyProvider.Provide<IPointService>(), DependencyProvider.Provide<IShapeService>()); });
                    DependencyProvider.Add(typeof(IOrderService), () => { return new OrderService(DependencyProvider.Provide<IOrderRepository>()); });
                    DependencyProvider.Add(typeof(IPointService), () => { return new PointService(DependencyProvider.Provide<IDataRepository<ApplicationCore.Entities.Point>>(), DependencyProvider.Provide<ITriangleService>()); });
                    DependencyProvider.Add(typeof(IShapeService), () => { return new ShapeService(DependencyProvider.Provide<IDataRepository<ApplicationCore.Entities.Shape>>(), DependencyProvider.Provide<IOrderService>()); });
                    DependencyProvider.Add(typeof(ITriangleService), () => { return new TriangleService(DependencyProvider.Provide<IDataRepository<ApplicationCore.Entities.Triangle>>(), DependencyProvider.Provide<IOrderService>()); });

                    break;

            }
        }
    }
}
