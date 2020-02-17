using System.Collections.Generic;

namespace Czeum.Core.DTOs.Paging
{
    public class PagedListDto<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Data { get; set; }
    }
}