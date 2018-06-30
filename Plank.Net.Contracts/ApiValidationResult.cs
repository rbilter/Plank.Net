using System.Collections.Generic;

namespace Plank.Net.Contracts
{
    public class ApiValidationResult
    {
        #region PROPERTIES

        public string Key { get; set; }

        public string Message { get; set; }

        public string Tag { get; set; }

        public object Target { get; set; }

        public IEnumerable<ApiValidationResult> NestedValidationResults { get; set; }

        #endregion
    }
}
