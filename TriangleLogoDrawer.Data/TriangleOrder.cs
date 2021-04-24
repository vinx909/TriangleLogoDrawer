using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.Data
{
    public class TriangleOrder
    {
        public int Id { get; set; }
        public int ShapeId { get; set; }
        public int TriangleOrigionalId { get; set; }
        public int TriangleFollowingId { get; set; }
    }
}
