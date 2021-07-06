using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.ApplicationCore.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BackgroundImagePath { get; set; }
        public virtual ICollection<Point> Points { get; set; }
        public virtual ICollection<Shape> Shapes { get; set; }
        public virtual ICollection<Triangle> Triangles { get; set; }
    }
}
