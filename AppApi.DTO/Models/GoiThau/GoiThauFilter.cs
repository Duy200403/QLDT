using AppApi.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DTO.Models.GoiThau
{
    public class GoiThauFilter : PagingRequestBase
    {
        public string Keyword { get; set; }          // tìm theo mã / tên gói
        public int? NamKeHoach { get; set; }         // lọc theo năm
        public int? TrangThaiHoSoId { get; set; }    // lọc theo trạng thái
    }
}
