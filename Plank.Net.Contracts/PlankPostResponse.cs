namespace Plank.Net.Contracts
{
    public class PlankPostResponse<T>
    {
        #region CONSTRUCTORS

        public PlankPostResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public T Item { get; set; }

        public PlankValidationResults ValidationResults { get; set; }

        #endregion
    }
}
