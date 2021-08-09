using TriangleLogoDrawer.ApplicationCore.Interfaces;
using TriangleLogoDrawer.ApplicationCore.Services;
using TriangleLogoDrawer.Infrastructure.Data;
using TriangleLogoDrawer.Infrastructure.Data.Sync;
using TriangleLogoDrawer.SimpleDependencyProvider;

namespace TriangleLogoDrawer.Data.Services.SimpleDependencyProvidedSetup
{
    public static class SimpleDependencyProviderSetup
    {
        private enum Options
        {
            InMemory,
            ApplicationCoreAndInfrastructure,
            ApplicationCoreAndInfrastructureSyncRepository 
        }
        private const Options option = Options.ApplicationCoreAndInfrastructureSyncRepository;

        public static void Setup()
        {
            switch (option)
            {
                case Options.ApplicationCoreAndInfrastructure:
                    ApplicationCoreAndInfrastructureBase();

                    DependencyProvider.Add<IDataRepository<ApplicationCore.Entities.Image>>(() => { return new DataRepository<ApplicationCore.Entities.Image>(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add<IDataRepository<ApplicationCore.Entities.Point>>(() => { return new DataRepository<ApplicationCore.Entities.Point>(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add<IDataRepository<ApplicationCore.Entities.Shape>>(() => { return new DataRepository<ApplicationCore.Entities.Shape>(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add<IDataRepository<ApplicationCore.Entities.Triangle>>(() => { return new DataRepository<ApplicationCore.Entities.Triangle>(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add<IOrderRepository>(() => { return new OrderRepository(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    break;

                case Options.ApplicationCoreAndInfrastructureSyncRepository:
                    ApplicationCoreAndInfrastructureBase();

                    DependencyProvider.Add<IDataRepository<ApplicationCore.Entities.Image>>(() => { return new DataSyncRepository<ApplicationCore.Entities.Image>(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add<IDataRepository<ApplicationCore.Entities.Point>>(() => { return new DataSyncRepository<ApplicationCore.Entities.Point>(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add<IDataRepository<ApplicationCore.Entities.Shape>>(() => { return new DataSyncRepository<ApplicationCore.Entities.Shape>(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add<IDataRepository<ApplicationCore.Entities.Triangle>>(() => { return new DataSyncRepository<ApplicationCore.Entities.Triangle>(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    DependencyProvider.Add<IOrderRepository>(() => { return new OrderSyncRepository(DependencyProvider.Get<TriangleDrawerDbContext>()); });
                    break;

            }
        }

        private static void ApplicationCoreAndInfrastructureBase()
        {
            DependencyProvider.Add<TriangleDrawerDbContext>(() => { return new TriangleDrawerDbContext(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TriangleDrawer;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"); });

            DependencyProvider.Add<IImageService>(() => { return new ImageService(DependencyProvider.Get<IDataRepository<ApplicationCore.Entities.Image>>(), DependencyProvider.Get<IPointService>(), DependencyProvider.Get<IShapeService>()); });
            DependencyProvider.Add<IOrderService>(() => { return new OrderService(DependencyProvider.Get<IOrderRepository>()); });
            DependencyProvider.Add<IPointService>(() => { return new PointService(DependencyProvider.Get<IDataRepository<ApplicationCore.Entities.Point>>(), DependencyProvider.Get<ITriangleService>()); });
            DependencyProvider.Add<IShapeService>(() => { return new ShapeService(DependencyProvider.Get<IDataRepository<ApplicationCore.Entities.Shape>>(), DependencyProvider.Get<IOrderService>()); });
            DependencyProvider.Add<ITriangleService>(() => { return new TriangleService(DependencyProvider.Get<IDataRepository<ApplicationCore.Entities.Triangle>>(), DependencyProvider.Get<IOrderService>()); });
        }
    }
}
