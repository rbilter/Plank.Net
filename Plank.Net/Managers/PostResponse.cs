using System;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Managers
{
    public sealed class PostResponse
    {
        #region CONSTRUCTORS

        public PostResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public int Id { get; set; }

        public ValidationResults ValidationResults { get; set; }

        #endregion
    }
}
