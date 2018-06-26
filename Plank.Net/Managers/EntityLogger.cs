using log4net;
using Plank.Net.Data;
using System;

namespace Plank.Net.Managers
{
    public sealed class EntityLogger<T> : ILogger<T> where T : Entity
    {
        #region MEMBERS

        private readonly ILog _logger = LogManager.GetLogger(typeof(EntityLogger<T>));

        #endregion

        #region METHODS

        public void Error(object message)
        {
            _logger.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            _logger.Error(message, exception);
        }

        public void Info(object message)
        {
            _logger.Info(message);
        }

        public void Info(object message, Exception exception)
        {
            _logger.Info(message, exception);
        }

        #endregion
    }
}
