using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.ApplicationCore.Entities
{
    public class Order
    {
        public int ShapeId { get; set; }
        public virtual Shape Shape { get; set; }
        public int TriangleId { get; set; }
        public virtual Triangle Triangle { get; set; }
        public int OrderNumber { get; set; }
    }
}
