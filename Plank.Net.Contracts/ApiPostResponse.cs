using System;

namespace Plank.Net.Contracts
{
    public class ApiPostResponse
    {
        #region CONSTRUCTORS

        public ApiPostResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public Guid Id { get; set; }

        public ApiValidationResults ValidationResults { get; set; }

        #endregion
    }
}
