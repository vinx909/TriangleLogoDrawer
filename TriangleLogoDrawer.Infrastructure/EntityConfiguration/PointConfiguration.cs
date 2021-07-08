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
    class PointConfiguration : IEntityTypeConfiguration<Point>
    {
        private const string pointTableName = "Points";

        public void Configure(EntityTypeBuilder<Point> builder)
        {
            builder.ToTable(pointTableName);
            builder.HasKey(p => p.Id);
            builder.HasOne(p => p.Image).WithMany(i => i.Points).HasForeignKey(p => p.ImageId);
        }
    }
}
