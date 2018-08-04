using Plank.Net.Data;
using System;
using System.Linq.Expressions;

namespace Plank.Net.Managers
{
    public interface IEntityManager<TEntity> where TEntity : Entity
    {
        #region METHODS

        PostResponse Create(TEntity entity);

        PostResponse Delete(Guid id);

        GetResponse<TEntity> Get(Guid id);

        PostEnumerableResponse<TEntity> Search(Expression<Func<TEntity, bool>> expression, int pageNumber, int pageSize);

        PostResponse Update(TEntity entity);

        PostResponse Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties);

        #endregion
    }
}
