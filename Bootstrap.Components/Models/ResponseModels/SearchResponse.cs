using System.Collections.Generic;

namespace Bootstrap.Components.Models.ResponseModels
{
    public class SearchResponse<T> : DataResponse<IEnumerable<T>>
    {
        public int TotalCount { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}