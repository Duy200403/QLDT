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
    [Table("VatTu")]
    public class VatTu : AuditEntity<Guid>
    {
        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaVatTu { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(500)")]
        public string TenVatTu { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(50)")]
        public string? DonViTinh { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? ThongSoKyThuat { get; set; }

        // === THÊM MỚI 2 TRƯỜNG NÀY ===
        public Guid? NganhId { get; set; }
        [ForeignKey(nameof(NganhId))]
        public virtual Nganh? Nganh { get; set; }

        public Guid? KhoId { get; set; }
        [ForeignKey(nameof(KhoId))]
        public virtual Kho? Kho { get; set; }
        // ==============================
    }
}
