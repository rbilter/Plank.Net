using Plank.Net.Data;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Managers
{
    public interface IEntityManager<TEntity> where TEntity : IEntity
    {
        #region METHODS

        Task<PostResponse<TEntity>> CreateAsync(TEntity entity);

        Task<DeleteResponse> DeleteAsync(int id);

        Task<GetResponse<TEntity>> GetAsync(int id);

        Task<PostEnumerableResponse<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        Task<PostResponse<TEntity>> UpdateAsync(TEntity entity);

        Task<PostResponse<TEntity>> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
