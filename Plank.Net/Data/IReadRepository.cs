using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public interface IReadRepository<T>
    {
        #region METHODS

        Task<T> GetAsync(int id);

        Task<IPagedList<T>> SearchAsync(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> includes, int pageNumber, int pageSize);

        #endregion
    }
}
