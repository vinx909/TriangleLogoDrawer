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
    class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        private const string imageTableName = "Images";

        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable(imageTableName);
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Name).HasMaxLength(50).IsRequired();
            builder.Property(i => i.BackgroundImagePath).HasMaxLength(255);
            builder.HasMany(i => i.Points).WithOne(p => p.Image);
            builder.HasMany(i => i.Shapes).WithOne(s => s.Image);
            builder.HasMany(i => i.Triangles).WithOne(t => t.Image);
        }
    }
}
