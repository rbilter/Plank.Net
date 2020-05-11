using Plank.Net.Contracts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Managers
{
    public interface IReadManager<T>
    {
        #region METHODS

        Task<PlankGetResponse<T>> GetAsync(int id);

        Task<PlankEnumerableResponse<T>> SearchAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize);

        #endregion
    }
}
