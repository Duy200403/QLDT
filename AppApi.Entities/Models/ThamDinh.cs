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
    [Table("ThamDinh")]
    public class ThamDinh : AuditEntity<Guid>
    {
        public ThamDinh()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        // FK -> TienTrinhHoSo (có thể null)
        public Guid? TienTrinhHoSoId { get; set; }

        [ForeignKey(nameof(TienTrinhHoSoId))]
        public virtual TienTrinhHoSo? TienTrinhHoSo { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string LoaiThamDinh { get; set; } = null!;   // KHL_CNT / HSMT / KQ_LCNT / HOP_DONG...

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? NoiDung { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? KetLuan { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? KienNghi { get; set; }

        public DateTime NgayThamDinh { get; set; } = DateTime.Now;

        // FK -> Account (NguoiThamDinhId trong SQL)
        public Guid? NguoiThamDinhId { get; set; }

        [ForeignKey(nameof(NguoiThamDinhId))]
        public virtual Account? NguoiThamDinh { get; set; }
    }
}
