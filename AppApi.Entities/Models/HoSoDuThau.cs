using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("HoSoDuThau")]
    public class HoSoDuThau : AuditEntity<Guid>
    {
        public HoSoDuThau()
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

        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string Loai { get; set; } = null!;   // BAO_GIA / HSDT

        public decimal? GiaDuThau { get; set; }

        public decimal? GiaDeXuat { get; set; }

        public decimal? GiaSauDieuChinh { get; set; }

        public decimal? DiemKyThuat { get; set; }

        public decimal? DiemTaiChinh { get; set; }

        public int? XepHang { get; set; }

        [Column(TypeName = "NVARCHAR(30)")]
        public string? KetLuan { get; set; }        // DAT / KHONG_DAT / TRUNG_THAU...

        [Column(TypeName = "NVARCHAR(20)")]
        public string? TrangThai { get; set; }      // DA_NOP / DA_MO / DANG_DANH_GIA...
    }
}
