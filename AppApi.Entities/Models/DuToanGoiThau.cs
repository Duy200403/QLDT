using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("DuToanGoiThau")]
    public class DuToanGoiThau : AuditEntity<Guid>
    {
        public DuToanGoiThau()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        public decimal GiaTriTruocThue { get; set; }

        public decimal TienThue { get; set; }

        public decimal GiaTriSauThue { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? GhiChu { get; set; }

        // FK -> Account (NguoiLapId trong SQL)
        public Guid? NguoiLapId { get; set; }

        [ForeignKey(nameof(NguoiLapId))]
        public virtual Account? NguoiLap { get; set; }

        public DateTime NgayLap { get; set; } = DateTime.Now;

        // 1 dự toán -> N dòng chi tiết
        public virtual ICollection<DuToanChiTiet> DuToanChiTiets { get; set; } = new HashSet<DuToanChiTiet>();
    }
}
