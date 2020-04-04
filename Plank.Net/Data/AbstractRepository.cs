using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public abstract class AbstractRepository<T> : IRepository<T>
    {
        #region PROPERTIES

        public IRepository<T> Next { get; set; }

        #endregion

        #region METHODS

        public abstract Task CreateAsync(T entity);

        public abstract Task DeleteAsync(int id);

        public abstract Task<T> GetAsync(int id);

        public IRepository<T> RegisterNext(IRepository<T> next)
        {
            Next = next;
            return Next;
        }

        public abstract Task<IPagedList<T>> SearchAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);

        public abstract Task UpdateAsync(T entity);

        public abstract Task UpdateAsync(T entity, params Expression<Func<T, object>>[] properties);

        #endregion
    }
}
