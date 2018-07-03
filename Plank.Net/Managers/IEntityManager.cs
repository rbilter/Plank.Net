using Plank.Net.Data;
using System;
using System.Linq.Expressions;

namespace Plank.Net.Managers
{
    public interface IEntityManager<T> where T : Entity
    {
        #region METHODS

        PostResponse Create(T entity);

        PostResponse Delete(Guid id);

        GetResponse<T> Get(Guid id);

        PostEnumerableResponse<T> Search(T entity, int pageNumber, int pageSize);

        PostResponse Update(T entity);

        PostResponse Update(T entity, params Expression<Func<T, object>>[] properties);

        #endregion
    }
}
