using Plank.Net.Contracts;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Managers
{
    public interface IWriteManager<T>
    {
        #region METHODS

        Task<PlankPostResponse<T>> AddAsync(T item);

        Task<PlankDeleteResponse> DeleteAsync(int id);

        Task<PlankPostResponse<T>> UpdateAsync(T item);

        Task<PlankPostResponse<T>> UpdateAsync(T item, params Expression<Func<T, object>>[] properties);

        #endregion
    }
}
