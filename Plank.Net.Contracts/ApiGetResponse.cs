using System;

namespace Plank.Net.Contracts
{
    public class ApiGetResponse<T>
    {
        #region MEMBERS

        private T _item;

        #endregion

        #region CONSTRUCTORS

        public ApiGetResponse()
        {
            _item = (T)Activator.CreateInstance(typeof(T));
        }

        #endregion

        #region PROPERTIES

        public T Item
        {
            get { return _item; }
            set { _item = value; }
        }

        public bool IsValid { get; set; }

        public string Message { get; set; }

        #endregion
    }
}
