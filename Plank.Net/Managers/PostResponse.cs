using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Plank.Net.Managers
{
    public sealed class PostResponse<T>
    {
        #region CONSTRUCTORS

        public PostResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public T Item { get; set; }

        public ValidationResults ValidationResults { get; set; }

        #endregion
    }
}
