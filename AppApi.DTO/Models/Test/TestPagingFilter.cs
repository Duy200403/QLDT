
using AppApi.DTO.Common;

namespace AppApi.DTO.Models.Test
{
    public class TestPagingFilter : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}