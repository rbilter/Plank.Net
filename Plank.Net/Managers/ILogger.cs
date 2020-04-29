using System;

namespace Plank.Net.Managers
{
    public interface ILogger
    {
        #region METHODS

        void ErrorMessage(object message);

        void ErrorMessage(object message, Exception exception);

        void InfoMessage(object message);

        void InfoMessage(object message, Exception exception);

        #endregion
    }
}
