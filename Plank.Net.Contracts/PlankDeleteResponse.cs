namespace Plank.Net.Contracts
{
    public class PlankDeleteResponse
    {
        #region CONSTRUCTORS

        public PlankDeleteResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public int Id { get; set; }

        public PlankValidationResults ValidationResults { get; set; }

        #endregion
    }
}
