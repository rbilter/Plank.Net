using PagedList;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public interface IEntityRepository<TEntity> where TEntity : IEntity
    {
        #region PROPERTIES

        IEntityRepository<TEntity> Next { get; set; }

        #endregion

        #region METHODS

        Task<int> CreateAsync(TEntity entity);

        Task<int> DeleteAsync(int id);

        Task<TEntity> GetAsync(int id);

        IEntityRepository<TEntity> RegisterNext(IEntityRepository<TEntity> next);

        Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        Task<int> UpdateAsync(TEntity entity);

        Task<int> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
