using System.Collections.Generic;

namespace Plank.Net.Contracts
{
    public class PlankBulkPostResponse<T>
    {
        #region CONSTRUCTORS

        public PlankBulkPostResponse()
            : this(new List<(T, PlankValidationResultCollection)>())
        {

        }

        public PlankBulkPostResponse(IEnumerable<(T, PlankValidationResultCollection)> validationResults)
        {
            Items = validationResults;
        }

        #endregion

        #region PROPERTIES

        public IEnumerable<(T Item, PlankValidationResultCollection ValidationResults)> Items { get; } = new List<(T, PlankValidationResultCollection)>();

        #endregion
    }
}
