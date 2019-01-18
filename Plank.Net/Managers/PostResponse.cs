using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Plank.Net.Data;

namespace Plank.Net.Managers
{
    public sealed class PostResponse<TEntity> where TEntity : IEntity
    {
        #region CONSTRUCTORS

        public PostResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public TEntity Item { get; set; }

        public ValidationResults ValidationResults { get; set; }

        #endregion
    }
}
