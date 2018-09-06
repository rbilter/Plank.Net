using Plank.Net.Data;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Managers
{
    public interface IEntityManager<TEntity> where TEntity : Entity
    {
        #region METHODS

        Task<PostResponse> CreateAsync(TEntity entity);

        Task<PostResponse> DeleteAsync(int id);

        Task<GetResponse<TEntity>> GetAsync(int id);

        Task<PostEnumerableResponse<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        Task<PostResponse> UpdateAsync(TEntity entity);

        Task<PostResponse> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
