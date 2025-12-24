using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("NghiemThu")]
    public class NghiemThu : AuditEntity<Guid>
    {
        public NghiemThu()
        {
        }

        // FK -> HopDong
        public Guid HopDongId { get; set; }

        [ForeignKey(nameof(HopDongId))]
        public virtual HopDong HopDong { get; set; } = null!;

        public DateTime? NgayNghiemThu { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? NoiDung { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? KetQua { get; set; }

        // FK -> FileDinhKem (biên bản nghiệm thu)
        public Guid? FileBienBanId { get; set; }

        [ForeignKey(nameof(FileBienBanId))]
        public virtual FileDinhKem? FileBienBan { get; set; }
    }
}