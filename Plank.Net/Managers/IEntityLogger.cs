using Plank.Net.Data;
using System;

namespace Plank.Net.Managers
{
    public interface ILogger<T> where T : Entity
    {
        #region METHODS

        void Error(object message);

        void Error(object message, Exception exception);

        void Info(object message);

        void Info(object message, Exception exception);

        #endregion
    }
}
