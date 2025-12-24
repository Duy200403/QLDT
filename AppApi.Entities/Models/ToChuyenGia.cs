using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("ToChuyenGia")]
    public class ToChuyenGia : AuditEntity<Guid>
    {
        public ToChuyenGia()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(50)")]
        public string? LoaiTo { get; set; }           // TO_THAM_DINH / TO_CHUYEN_GIA...

        [Column(TypeName = "NVARCHAR(50)")]
        public string? SoQuyetDinh { get; set; }

        public DateTime? NgayQuyetDinh { get; set; }

        // FK -> FileDinhKem (FileQuyetDinhId trong SQL) – có thể null
        public Guid? FileQuyetDinhId { get; set; }

        [ForeignKey(nameof(FileQuyetDinhId))]
        public virtual FileDinhKem? FileQuyetDinh { get; set; }
    }
}