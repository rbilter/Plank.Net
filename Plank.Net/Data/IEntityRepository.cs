using PagedList;
using System;
using System.Linq.Expressions;

namespace Plank.Net.Data
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        #region METHODS

        Guid Create(TEntity entity);

        Guid Delete(Guid id);

        TEntity Get(Guid id);

        IPagedList<TEntity> Search(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        Guid Update(TEntity entity);

        Guid Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
