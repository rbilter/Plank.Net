namespace Plank.Net.Contracts
{
    public class PlankDeleteResponse
    {
        #region CONSTRUCTORS

        public PlankDeleteResponse()
            : this(new PlankValidationResultCollection())
        {

        }

        public PlankDeleteResponse(PlankValidationResultCollection validationResults)
        {
            ValidationResults = new PlankValidationResultCollection();
            ValidationResults.AddRange(validationResults);
        }

        #endregion

        #region PROPERTIES

        public int Id { get; set; }

        public PlankValidationResultCollection ValidationResults { get; } = new PlankValidationResultCollection();

        #endregion
    }
}
