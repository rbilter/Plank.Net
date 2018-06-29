using System;
using System.Collections.Generic;

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

        public IEnumerable<PlankValidationResult> ValidationResults { get; set; }

        #endregion
    }
}
