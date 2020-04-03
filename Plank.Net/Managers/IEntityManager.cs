using Plank.Net.Contracts;
using Plank.Net.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Managers
{
    public interface IEntityManager<TEntity> where TEntity : IEntity
    {
        #region METHODS

        Task<PlankPostResponse<TEntity>> CreateAsync(TEntity entity);

        Task<PlankDeleteResponse> DeleteAsync(int id);

        Task<PlankGetResponse<TEntity>> GetAsync(int id);

        Task<PlankEnumerableResponse<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity entity);

        Task<PlankPostResponse<TEntity>> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
