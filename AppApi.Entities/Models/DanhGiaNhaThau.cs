using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("DanhGiaNhaThau")]
    public class DanhGiaNhaThau : AuditEntity<Guid>
    {
        public DanhGiaNhaThau()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        // FK -> NhaThau
        public Guid NhaThauId { get; set; }

        [ForeignKey(nameof(NhaThauId))]
        public virtual NhaThau NhaThau { get; set; } = null!;

        public DateTime? ThoiDiemDanhGia { get; set; }

        /// <summary>
        /// Lưu JSON danh sách tiêu chí + điểm chi tiết
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? TieuChiJson { get; set; }

        public decimal? DiemTong { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? NhanXet { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? XepLoai { get; set; }   // VD: XUAT_SAC / TOT / TRUNG_BINH...
    }
}