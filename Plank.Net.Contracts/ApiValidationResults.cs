using System.Collections.Generic;

namespace Plank.Net.Contracts
{
    public class ApiValidationResults : List<ApiValidationResult>
    {
        #region PROPERTIES

        public bool IsValid { get { return this.Count == 0; } }

        #endregion
    }
}
