using Plank.Net.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;

namespace Plank.Net.Data
{
    public interface IEntityRepository<TEntity> where TEntity : IEntity
    {
        #region PROPERTIES

        IEntityRepository<TEntity> Next { get; set; }

        #endregion

        #region METHODS

        Task CreateAsync(TEntity entity);

        Task DeleteAsync(int id);

        Task<TEntity> GetAsync(int id);

        IEntityRepository<TEntity> RegisterNext(IEntityRepository<TEntity> next);

        Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        Task UpdateAsync(TEntity entity);

        Task UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
