using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DTO.Models.BuocQuyTrinh
{
    public class BuocQuyTrinhResponse
    {
        public Guid Id { get; set; }
        public string MaBuoc { get; set; } = null!;
        public string TenBuoc { get; set; } = null!;
        public int ThuTu { get; set; }
        public string? GhiChu { get; set; }
        public string TrangThai { get; set; } = null!;
    }
}
