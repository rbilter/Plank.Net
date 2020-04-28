namespace Plank.Net.Contracts
{
    public class PlankPostResponse<T>
    {
        #region CONSTRUCTORS

        public PlankPostResponse()
            : this(new PlankValidationResultCollection())
        {
        }

        public PlankPostResponse(PlankValidationResultCollection validationResults)
        {
            ValidationResults = new PlankValidationResultCollection();
            ValidationResults.AddRange(validationResults);
        }

        #endregion

        #region PROPERTIES

        public T Item { get; set; }

        public PlankValidationResultCollection ValidationResults { get; }

        #endregion
    }
}
