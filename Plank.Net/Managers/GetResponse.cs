using System;

namespace Plank.Net.Managers
{
    public class GetResponse<T>
    {
        #region MEMBERS

        private T _item;

        #endregion

        #region CONSTRUCTORS

        public GetResponse()
            : this((T)Activator.CreateInstance(typeof(T)))
        {

        }

        public GetResponse(T item)
        {
            _item = item;
        }

        #endregion

        #region PROPERTIES

        public T Item { get { return _item; } }

        public bool IsValid { get; set; }

        public string Message { get; set; }

        #endregion
    }
}
