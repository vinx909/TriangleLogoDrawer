using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.ApplicationCore.Entities
{
    public class Triangle
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public virtual Image Image { get; set; }
        public int PointOneId { get; set; }
        public virtual Point PointOne { get; set; }
        public int PointTwoId { get; set; }
        public virtual Point PointTwo { get; set; }
        public int PointThreeId { get; set; }
        public virtual Point PointThree { get; set; }
        public virtual Order order { get; set; }
    }
}
