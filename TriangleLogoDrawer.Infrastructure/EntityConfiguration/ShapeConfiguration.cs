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
    class ShapeConfiguration : IEntityTypeConfiguration<Shape>
    {
        private const string shapeTableName = "Shapes";

        public void Configure(EntityTypeBuilder<Shape> builder)
        {
            builder.ToTable(shapeTableName);
            builder.HasKey(s => s.Id);
            builder.HasOne(s => s.Image).WithMany(i => i.Shapes).HasForeignKey(s => s.ImageId);
            builder.Property(s => s.Name).HasMaxLength(50).IsRequired();
            builder.HasMany(s => s.Orders).WithOne(o => o.Shape);
        }
    }
}
