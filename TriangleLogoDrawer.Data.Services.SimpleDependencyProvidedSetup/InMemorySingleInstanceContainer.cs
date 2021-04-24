using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data.Services.SimpleDependencyProvidedSetup
{
    internal class InMemorySingleInstanceContainer
    {
        internal IImageData InMemoryImageData { get; set; }
        internal IPointData InMemoryPointData { get; set; }
        internal IShapeData InMemoryShapeData { get; set; }
        internal ITriangleData InMemoryTriangleData { get; set; }
        internal ITriangleOrderData InMemoryTriangleOrderData { get; set; }
    }
}
