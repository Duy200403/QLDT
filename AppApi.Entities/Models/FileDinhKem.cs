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
    [Table("FileDinhKem")]
    public class FileDinhKem : AuditEntity<Guid>
    {
        public FileDinhKem()
        {
        }

        // FK -> HoSoThau (file chung cho hồ sơ)
        public Guid? HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau? HoSoThau { get; set; }

        // FK -> TienTrinhHoSo (file gắn theo từng bước)
        public Guid? TienTrinhHoSoId { get; set; }

        [ForeignKey(nameof(TienTrinhHoSoId))]
        public virtual TienTrinhHoSo? TienTrinhHoSo { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? LoaiFile { get; set; }         // DE_XUAT_KT / KHL_CNT / HSMT / HSDT / BIEN_BAN / ...

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenHienThi { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenFileGoc { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(500)")]
        public string DuongDan { get; set; } = null!;

        public DateTime NgayUpload { get; set; } = DateTime.Now;

        // FK -> Account (người upload)
        public Guid? NguoiUploadId { get; set; }

        [ForeignKey(nameof(NguoiUploadId))]
        public virtual Account? NguoiUpload { get; set; }
    }
}
