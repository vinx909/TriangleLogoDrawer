using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.Infrastructure.Data
{
    public class TriangleDrawerDbContext : DbContext
    {
        private const string imageTableName = "Images";
        private const string orderTableName = "Orders";
        private const string pointTableName = "Points";
        private const string shapeTableName = "Shapes";
        private const string triangleTableName = "Triangles";

        private const string imageId = "ImageId";
        private const string pointOneId = "PointOneId";
        private const string pointTwoId = "PointTwoId";
        private const string pointThreeId = "PointThreeId";
        private const string triangleId = "TriangleId";
        private const string shapeId = "ShapeId";

        private readonly string connectionString;

        public TriangleDrawerDbContext()
        {
            connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TriangleDrawer;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        }
        public TriangleDrawerDbContext(string connectionString) : base()
        {
            this.connectionString = connectionString;
        }
        public TriangleDrawerDbContext(DbContextOptions<TriangleDrawerDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (connectionString != null)
            {
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Image>(i => 
            {
                i.ToTable(imageTableName);
                i.HasKey(i => i.Id);
                i.Property(i => i.Name).HasMaxLength(50).IsRequired();
                i.Property(i => i.BackgroundImagePath).HasMaxLength(255);
                i.HasMany(i => i.Points).WithOne(p => p.Image);
                i.HasMany(i => i.Shapes).WithOne(s => s.Image);
                i.HasMany(i => i.Triangles).WithOne(t => t.Image);
            });

            modelBuilder.Entity<Order>(o =>
            {
                o.ToTable(orderTableName);
                o.HasKey(o => new { o.ShapeId, o.TriangleId });
                o.HasOne(o => o.Shape).WithMany(s => s.Orders).HasForeignKey(o => o.ShapeId);
                o.HasOne(o => o.Triangle).WithOne().HasForeignKey<Order>(o => o.TriangleId).OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Point>(p =>
            {
                p.ToTable(pointTableName);
                p.HasKey(p => p.Id);
                p.HasOne(p => p.Image).WithMany(i => i.Points).HasForeignKey(p => p.ImageId);
            });

            modelBuilder.Entity<Shape>(s =>
            {
                s.ToTable(shapeTableName);
                s.HasKey(s => s.Id);
                s.HasOne(s => s.Image).WithMany(i => i.Shapes).HasForeignKey(s => s.ImageId);
                s.Property(s => s.Name).HasMaxLength(50).IsRequired();
                s.HasMany(s => s.Orders).WithOne(o => o.Shape);
            });

            modelBuilder.Entity<Triangle>(t =>
            {
                t.ToTable(triangleTableName);
                t.HasKey(t => t.Id);
                t.HasOne(t => t.Image).WithMany(i => i.Triangles).HasForeignKey(t => t.ImageId);
                t.HasOne(t => t.PointOne).WithMany(p => p.TrianglesPointOne).HasForeignKey(t => t.PointOneId).OnDelete(DeleteBehavior.NoAction);
                t.HasOne(t => t.PointTwo).WithMany(p => p.TrianglesPointTwo).HasForeignKey(t => t.PointTwoId).OnDelete(DeleteBehavior.NoAction);
                t.HasOne(t => t.PointThree).WithMany(p => p.TrianglesPointThree).HasForeignKey(t => t.PointThreeId).OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
