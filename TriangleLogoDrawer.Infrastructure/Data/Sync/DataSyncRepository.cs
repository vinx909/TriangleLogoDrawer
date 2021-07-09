using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Interfaces;

namespace TriangleLogoDrawer.Infrastructure.Data.Sync
{
    public class DataSyncRepository<T> : IDataRepository<T> where T : class
    {
        protected readonly TriangleDrawerDbContext triangleDrawerDbContext;

        public DataSyncRepository(TriangleDrawerDbContext triangleDrawerDbContext)
        {
            this.triangleDrawerDbContext = triangleDrawerDbContext;
        }

        public Task<T> Create(T @object)
        {
            triangleDrawerDbContext.Add(@object);
            triangleDrawerDbContext.SaveChanges();
            return Task.FromResult(@object);
        }

        public Task Edit(T @object)
        {
            triangleDrawerDbContext.Update(@object);
            triangleDrawerDbContext.SaveChanges();
            return Task.CompletedTask;
        }

        public Task<T> Get(int id)
        {
            return Task.FromResult(triangleDrawerDbContext.Find<T>(id));
        }

        public Task<IEnumerable<T>> GetAll()
        {
            return Task.FromResult((IEnumerable<T>)triangleDrawerDbContext.Set<T>());
        }

        public Task<IEnumerable<T>> GetAll(Func<T, bool> searchQuiry)
        {
            return Task.FromResult((IEnumerable<T>)triangleDrawerDbContext.Set<T>().Where(searchQuiry));
        }

        public Task Remove(T @object)
        {
            triangleDrawerDbContext.Remove(@object);
            triangleDrawerDbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}
