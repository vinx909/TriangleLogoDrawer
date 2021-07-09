using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TriangleLogoDrawer.ApplicationCore.Entities;

namespace TriangleLogoDrawer.ApplicationCore.Interfaces
{
    public interface IDataRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAll();
        public Task<IEnumerable<T>> GetAll(Func<T, bool> searchQuiry);
        public Task<T> Get(int id);
        public Task<T> Create(T @object);
        public Task Edit(T @object);
        public Task Remove(T @object);
    }
}
