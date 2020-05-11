using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public interface IReadRepository<T>
    {
        #region METHODS

        Task<T> GetAsync(int id);

        Task<IPagedList<T>> SearchAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);

        #endregion
    }
}
