using System.Collections.Generic;

namespace Plank.Net.Contracts
{
    public class PlankValidationResultCollection : List<PlankValidationResult>
    {
        #region PROPERTIES

        public bool IsValid { get { return Count == 0; } }

        #endregion
    }
}
