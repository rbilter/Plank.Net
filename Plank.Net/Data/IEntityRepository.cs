using PagedList;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        #region METHODS

        Task<int> CreateAsync(TEntity entity);

        Task<int> DeleteAsync(int id);

        Task<TEntity> GetAsync(int id);

        Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        Task<int> UpdateAsync(TEntity entity);

        Task<int> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
