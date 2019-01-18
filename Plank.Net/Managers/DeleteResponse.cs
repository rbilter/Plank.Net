using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Managers
{
    public sealed class DeleteResponse
    {
        #region CONSTRUCTORS

        public DeleteResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public int Id { get; set; }

        public ValidationResults ValidationResults { get; set; }

        #endregion
    }
}
