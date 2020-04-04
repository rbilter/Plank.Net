using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public interface IRepository<T>
    {
        #region PROPERTIES

        IRepository<T> Next { get; set; }

        #endregion

        #region METHODS

        Task CreateAsync(T entity);

        Task DeleteAsync(int id);

        Task<T> GetAsync(int id);

        IRepository<T> RegisterNext(IRepository<T> next);

        Task<IPagedList<T>> SearchAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);

        Task UpdateAsync(T entity);

        Task UpdateAsync(T entity, params Expression<Func<T, object>>[] properties);

        #endregion
    }
}
