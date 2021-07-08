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
    class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        private const string orderTableName = "Orders";

        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable(orderTableName);
            builder.HasKey(o => new { o.ShapeId, o.TriangleId });
            builder.HasOne(o => o.Shape).WithMany(s => s.Orders).HasForeignKey(o => o.ShapeId);
            builder.HasOne(o => o.Triangle).WithOne().HasForeignKey<Order>(o => o.TriangleId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
