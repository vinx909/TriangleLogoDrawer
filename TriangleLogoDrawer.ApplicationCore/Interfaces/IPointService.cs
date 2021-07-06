using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.ApplicationCore.Interfaces
{
    public interface IPointService
    {
        public Task Create(Point point);
        public Task Remove(int pointId);
        public Task Remove(Point point);
    }
}
