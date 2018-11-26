using System.Collections.Generic;

namespace Plank.Net.Contracts
{
    public class PlankValidationResults : List<PlankValidationResult>
    {
        #region PROPERTIES

        public bool IsValid { get { return this.Count == 0; } }

        #endregion
    }
}
