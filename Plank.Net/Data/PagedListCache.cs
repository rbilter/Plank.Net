using System.Collections.Generic;

namespace Plank.Net.Data
{
    public class PagedListCache
    {
        #region PROPERTIES

        public List<int> Ids { get; set; }

        public int PageNumber { get; set; }

        public int TotalItemCount { get; set; }

        #endregion
    }
}
