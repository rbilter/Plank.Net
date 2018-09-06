using Plank.Net.Data;
using System;

namespace Plank.Net.Managers
{
    public interface ILogger<TEntity> where TEntity : IEntity
    {
        #region METHODS

        void Error(object message);

        void Error(object message, Exception exception);

        void Info(object message);

        void Info(object message, Exception exception);

        #endregion
    }
}
