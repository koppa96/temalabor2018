using System.Collections.Generic;

namespace Czeum.Core.DTOs.Paging
{
    public class RollListDto<TElement>
    {
        public bool HasMoreLeft { get; set; }
        public IEnumerable<TElement> Data { get; set; }
    }
}