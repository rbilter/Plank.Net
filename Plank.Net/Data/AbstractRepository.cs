using PagedList;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Plank.Net.Data
{
    public abstract class AbstractRepository<TEntity> : IEntityRepository<TEntity> where TEntity : class, IEntity
    {
        #region PROPERTIES

        public IEntityRepository<TEntity> Next { get; set; }

        #endregion

        #region METHODS

        public abstract Task<int> CreateAsync(TEntity entity);

        public abstract Task<int> DeleteAsync(int id);

        public abstract Task<TEntity> GetAsync(int id);

        public IEntityRepository<TEntity> RegisterNext(IEntityRepository<TEntity> next)
        {
            Next = next;
            return Next;
        }

        public abstract Task<IPagedList<TEntity>> SearchAsync(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        public abstract Task<int> UpdateAsync(TEntity entity);

        public abstract Task<int> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
