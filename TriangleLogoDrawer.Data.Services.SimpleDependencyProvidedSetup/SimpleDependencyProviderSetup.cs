using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.Data.Services.Infrastructure.InMemory;
using TriangleLogoDrawer.SimpleDependencyProvider;

namespace TriangleLogoDrawer.Data.Services.SimpleDependencyProvidedSetup
{
    public static class SimpleDependencyProviderSetup
    {
        private enum options
        {
            InMemory
        }
        private const options option = options.InMemory;

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
                    DependencyProvider.Add(typeof(IPointData), imageProvider);

                    Func<IShapeData> shapeProvider = () => { return container.InMemoryShapeData; };
                    DependencyProvider.Add(typeof(IShapeData), imageProvider);

                    Func<ITriangleData> triangleProvider = () => { return container.InMemoryTriangleData; };
                    DependencyProvider.Add(typeof(ITriangleData), imageProvider);

                    Func<ITriangleOrderData> orderProvider = () => { return container.InMemoryTriangleOrderData; };
                    DependencyProvider.Add(typeof(ITriangleOrderData), imageProvider);
                    break;
            }
        }
    }
}
