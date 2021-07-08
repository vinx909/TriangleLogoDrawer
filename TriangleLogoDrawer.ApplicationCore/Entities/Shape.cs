using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.ApplicationCore.Entities
{
    public class Shape
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        internal IOrderedEnumerable<Order> GetOrder()
        {
            if(Orders != null)
            {
                return Orders.OrderBy(o => o.OrderNumber);
            }
            else
            {
                return (IOrderedEnumerable<Order>)new List<Order>();
            }
        }

        public bool HasOrders()
        {
            if(Orders != null)
            {
                return Orders.Any();
            }
            else
            {
                return false;
            }
        }
    }
}
