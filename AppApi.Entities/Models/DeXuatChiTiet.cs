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
    [Table("DeXuatChiTiet")]
    public class DeXuatChiTiet : AuditEntity<Guid>
    {
        public Guid DeXuatMuaSamId { get; set; }
        [ForeignKey(nameof(DeXuatMuaSamId))]
        public virtual DeXuatMuaSam DeXuatMuaSam { get; set; } = null!;

        // === SỬA ĐỔI QUAN TRỌNG: CHO PHÉP NULL ===
        public Guid? VatTuId { get; set; } // Null = Nhập tay, Có Guid = Chọn từ danh mục
        [ForeignKey(nameof(VatTuId))]
        public virtual VatTu? VatTu { get; set; }

        // === CÁC TRƯỜNG SNAPSHOT (LƯU CỨNG) ===
        // Nếu chọn danh mục: Tự điền từ bảng VatTu sang
        // Nếu nhập tay: Người dùng tự gõ vào đây
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

        // Số liệu đề xuất
        public decimal SoLuong { get; set; }

        // Thêm trường hiển thị Kho/Ngành lúc đề xuất (Optional - để báo cáo)
        [Column(TypeName = "NVARCHAR(255)")]
        public string? TenKhoDeXuat { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? TenNganhDeXuat { get; set; }
    }
}
