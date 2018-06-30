using System;

namespace Plank.Net.Contracts
{
    public class PlankPostResponse
    {
        #region CONSTRUCTORS

        public PlankPostResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public Guid Id { get; set; }

        public PlankValidationResults ValidationResults { get; set; }

        #endregion
    }
}
