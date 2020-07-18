using Plank.Net.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Managers
{
    public interface IReadManager<T>
    {
        #region METHODS

        Task<PlankGetResponse<T>> GetAsync(int id);

        Task<PlankEnumerableResponse<T>> SearchAsync(Expression<Func<T, bool>> expression, List<Expression<Func<T, object>>> includes, int pageNumber, int pageSize);

        #endregion
    }
}
