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
    [Table("DeXuatMuaSam")]
    public class DeXuatMuaSam : AuditEntity<Guid>
    {
        public DeXuatMuaSam()
        {
        }

        public int NamKeHoach { get; set; }                  // NamKeHoach

        [Column(TypeName = "NVARCHAR(50)")]
        public string? MaDeXuat { get; set; }                // MaDeXuat

        // FK -> DonVi (DonViDeXuatId INT trong SQL)
        public Guid DonViDeXuatId { get; set; }

        [ForeignKey(nameof(DonViDeXuatId))]
        public virtual DonVi DonViDeXuat { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenDeXuat { get; set; } = null!;       // TenDeXuat

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? LyDo { get; set; }                    // LyDo

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? MoTaChung { get; set; }               // MoTaChung

        public decimal TongGiaTriUocTinh { get; set; }       // TongGiaTriUocTinh

        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string TrangThai { get; set; } = "DRAFT";     // DRAFT / SUBMITTED / AGGREGATED

        // FK -> Account (NguoiTaoId INT trong SQL)
        public Guid? NguoiTaoId { get; set; }

        [ForeignKey(nameof(NguoiTaoId))]
        public virtual Account? NguoiTao { get; set; }

        // Quan hệ: 1 Đề xuất -> N chi tiết
        public virtual ICollection<DeXuatChiTiet> DeXuatChiTiets { get; set; } = new HashSet<DeXuatChiTiet>();

        // Quan hệ: 1 Đề xuất -> N mapping sang các gói thầu KH
        public virtual ICollection<GoiThauDeXuat> GoiThauDeXuats { get; set; } = new HashSet<GoiThauDeXuat>();
    }
}