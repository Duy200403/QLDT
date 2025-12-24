using System.Collections.Generic;

namespace AppApi.DTO.Common
{
    public class PagedResult<T> :PagedResultBase
    {
         public IEnumerable<T> Data { set; get; }
    }
}