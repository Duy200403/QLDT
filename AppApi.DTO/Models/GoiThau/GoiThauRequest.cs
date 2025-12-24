using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.DTO.Models.GoiThau
{
    public class GoiThauRequest
    {
        public GoiThauRequest() { }
        public string MaGoiThau { get; set; } = null!;
        public string TenGoiThau { get; set; } = null!;
        public Guid? KeHoachMuaSamId { get; set; }
        public Guid DonViChuTriId { get; set; }
        public string HinhThucChonNhaThau { get; set; } = null!;
        public string? LoaiGoiThau { get; set; }
        public decimal? GiaTriDuToan { get; set; }
        public string? NguonKinhPhi { get; set; }
        public int TrangThaiHoSoId { get; set; }
        public DateTime NgayKhoiTao { get; set; }
        public DateTime? NgayHoanThanh { get; set; }
        public string? GhiChu { get; set; }
    }
}
