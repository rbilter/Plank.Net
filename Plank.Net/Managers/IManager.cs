using System;

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

        #endregion
    }
}
