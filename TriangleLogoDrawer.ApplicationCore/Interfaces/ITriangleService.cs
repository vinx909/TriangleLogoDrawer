using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.ApplicationCore.Interfaces
{
    public interface ITriangleService
    {
        public Task Create(Triangle triangle);
        public Task Remove(int triangleId);
        public Task Remove(Triangle triangle);
    }
}
