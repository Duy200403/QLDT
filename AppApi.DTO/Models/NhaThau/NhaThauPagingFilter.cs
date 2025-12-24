using AppApi.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DTO.Models.NhaThau
{
    public class NhaThauPagingFilter : PagingRequestBase
    {
        public string? Keyword { get; set; } // tìm theo tên/mst/sđt/email
    }
}
