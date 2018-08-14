using PagedList;
using System;
using System.Linq.Expressions;

namespace Plank.Net.Data
{
    public interface IEntityRepository<TEntity> where TEntity : Entity
    {
        #region METHODS

        int Create(TEntity entity);

        int Delete(int id);

        TEntity Get(int id);

        IPagedList<TEntity> Search(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        int Update(TEntity entity);

        int Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
