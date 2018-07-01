using System.Collections;
using System.Collections.Generic;

namespace Plank.Net.Contracts
{
    public class ApiEnumerableResponse<T> : IEnumerable<T>
    {
        #region MEMBERS

        private List<T> _list;

        #endregion

        #region CONSTRUCTORS

        public ApiEnumerableResponse()
        {
            _list = new List<T>();
        }

        #endregion

        #region PROPERTIES

        public bool IsValid { get; set; }

        public string Message { get; set; }

        #endregion

        #region METHODS

        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        #endregion
    }
}
