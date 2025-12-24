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
    [Table("ThuMoiBaoGia")]
    public class ThuMoiBaoGia : AuditEntity<Guid>
    {
        public ThuMoiBaoGia()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string Loai { get; set; } = "BAO_GIA";   // BAO_GIA

        public DateTime? NgayGui { get; set; }

        public DateTime? HanNhan { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? NoiDungTomTat { get; set; }
    }
}
