using System.Collections.Generic;

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
