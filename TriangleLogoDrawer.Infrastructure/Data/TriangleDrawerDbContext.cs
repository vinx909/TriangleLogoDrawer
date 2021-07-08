using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.Infrastructure.EntityConfiguration;

namespace TriangleLogoDrawer.Infrastructure.Data
{
    public class TriangleDrawerDbContext : DbContext
    {
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

            modelBuilder.ApplyConfiguration(new ImageConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new PointConfiguration());
            modelBuilder.ApplyConfiguration(new ShapeConfiguration());
            modelBuilder.ApplyConfiguration(new TriangleConfiguration());
        }
    }
}
