﻿using System.Collections.Generic;

namespace Plank.Net.Managers
{
    public class PostEnumerableResponse<T>
    {
        #region CONSTRUCTORS

        public PostEnumerableResponse()
        {

        }

        #endregion

        #region PROPERTIES

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public bool IsFirstPage { get; set; }

        public bool IsLastPage { get; set; }

        public bool IsValid { get; set; }

        public IEnumerable<T> Items { get; set; }

        public string Message { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalItemCount { get; set; }

        #endregion
    }
}
