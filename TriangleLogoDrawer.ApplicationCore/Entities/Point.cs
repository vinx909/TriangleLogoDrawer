using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TriangleLogoDrawer.ApplicationCore.Entities
{
    public class Point
    {
        public int Id { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public List<Triangle> Triangles { get => (List<Triangle>)new List<Triangle>().Concat(TrianglesPointOne).Concat(TrianglesPointTwo).Concat(TrianglesPointThree); }
        public ICollection<Triangle> TrianglesPointOne { get; set; }
        public ICollection<Triangle> TrianglesPointTwo { get; set; }
        public ICollection<Triangle> TrianglesPointThree { get; set; }
    }
}
