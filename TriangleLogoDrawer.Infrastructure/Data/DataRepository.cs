using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace TriangleLogoDrawer.Infrastructure.Data
{
    public class DataRepository<T> : IDataRepository<T> where T : class
    {
        protected readonly TriangleDrawerDbContext triangleDrawerDbContext;

        public DataRepository(TriangleDrawerDbContext triangleDrawerDbContext)
        {
            this.triangleDrawerDbContext = triangleDrawerDbContext;
        }

        public async Task<T> Create(T @object)
        {
            triangleDrawerDbContext.Add<T>(@object);
            await triangleDrawerDbContext.SaveChangesAsync();
            return @object;
        }

        public async Task Edit(T @object)
        {
            triangleDrawerDbContext.Update<T>(@object);
            await triangleDrawerDbContext.SaveChangesAsync();
        }

        public async Task<T> Get(int id)
        {
            return await triangleDrawerDbContext.FindAsync<T>(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await triangleDrawerDbContext.Set<T>().ToListAsync(); ;
        }

        public async Task<IEnumerable<T>> GetAll(Func<T, bool> searchQuiry)
        {
            List<T> list = await triangleDrawerDbContext.Set<T>().ToListAsync();
            return list.Where(searchQuiry);
        }

        public async Task Remove(T @object)
        {
            triangleDrawerDbContext.Remove(@object);
            await triangleDrawerDbContext.SaveChangesAsync();
        }
    }
}
