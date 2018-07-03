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

        public ApiEnumerableResponse(List<T> list)
        {
            _list = new List<T>();
            _list.AddRange(list);
        }

        public ApiEnumerableResponse(IEnumerable<T> list)
        {
            _list = new List<T>();
            _list.AddRange(list);
        }

        #endregion

        #region PROPERTIES

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public bool IsValid { get; set; }

        public string Message { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

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
