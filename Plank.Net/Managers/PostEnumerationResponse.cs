using System.Collections;
using System.Collections.Generic;

namespace Plank.Net.Managers
{
    public class PostEnumerationResponse<T> : IEnumerable<T>
    {
        #region MEMBERS

        private List<T> _list;

        #endregion

        #region CONSTRUCTORS

        public PostEnumerationResponse()
            : this(new List<T>())
        {

        }

        public PostEnumerationResponse(List<T> list)
        {
            _list = new List<T>();
            _list.AddRange(list);
        }

        public PostEnumerationResponse(IEnumerable<T> list)
        {
            _list = new List<T>();
            _list.AddRange(list);
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
