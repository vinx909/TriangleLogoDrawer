using System.Collections.Generic;
using System.Linq;

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

        public IOrderedEnumerable<Order> GetOrder(int shapeId)
        {
            Shape shape = GetShape(shapeId);
            if (shape != null)
            {
                return shape.GetOrder();
            }
            else
            {
                return (IOrderedEnumerable<Order>)new List<Order>();
            }
        }

        private Shape GetShape(int shapeId)
        {
            return Shapes.FirstOrDefault(s => s.Id == shapeId);
        }

        public bool HasOrders(int shapeId)
        {
            Shape shape = GetShape(shapeId);
            if(shape != null)
            {
                return shape.HasOrders();
            }
            else
            {
                return false;
            }
        }
    }
}
