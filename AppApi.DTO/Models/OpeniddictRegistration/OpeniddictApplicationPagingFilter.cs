
using AppApi.DTO.Common;

namespace AppApi.DTO.Models.OpeniddictRegistration
{
    public class OpeniddictApplicationPagingFilter : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}