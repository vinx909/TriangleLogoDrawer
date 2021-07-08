using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.Infrastructure.EntityConfiguration
{
    class TriangleConfiguration : IEntityTypeConfiguration<Triangle>
    {
        private const string triangleTableName = "Triangles";

        public void Configure(EntityTypeBuilder<Triangle> builder)
        {
            builder.ToTable(triangleTableName);
            builder.HasKey(t => t.Id);
            builder.HasOne(t => t.Image).WithMany(i => i.Triangles).HasForeignKey(t => t.ImageId);
            builder.HasOne(t => t.PointOne).WithMany(p => p.TrianglesPointOne).HasForeignKey(t => t.PointOneId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(t => t.PointTwo).WithMany(p => p.TrianglesPointTwo).HasForeignKey(t => t.PointTwoId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(t => t.PointThree).WithMany(p => p.TrianglesPointThree).HasForeignKey(t => t.PointThreeId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
