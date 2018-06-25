using System;
using System.Linq.Expressions;

namespace Plank.Net.Managers
{
    public interface IManager<T>
    {
        #region METHODS

        PostResponse Create(T entity);

        PostResponse Delete(Guid id);

        GetResponse<T> Get(Guid id);

        PostEnumerationResponse<T> Search(T entity);

        PostResponse Update(T entity);

        PostResponse Update(T entity, params Expression<Func<T, object>>[] properties);

        #endregion
    }
}
