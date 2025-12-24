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
    [Table("ThuMoiThau")]
    public class ThuMoiThau : AuditEntity<Guid>
    {
        public ThuMoiThau()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string Loai { get; set; } = "HSMT";   // HSMT / HSYC

        public DateTime? NgayGui { get; set; }

        public DateTime? HanNhan { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? NoiDungTomTat { get; set; }
    }
}
