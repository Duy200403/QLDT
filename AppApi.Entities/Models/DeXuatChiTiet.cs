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
        public DeXuatChiTiet()
        {
        }

        // FK -> DeXuatMuaSam (DeXuatId INT trong SQL)
        public Guid DeXuatId { get; set; }

        [ForeignKey(nameof(DeXuatId))]
        public virtual DeXuatMuaSam DeXuat { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenHangHoaDichVu { get; set; } = null!;  // TenHangHoaDichVu

        [Column(TypeName = "NVARCHAR(100)")]
        public string? MaHang { get; set; }                     // MaHang

        public decimal SoLuong { get; set; }                    // SoLuong

        [Column(TypeName = "NVARCHAR(50)")]
        public string? DonViTinh { get; set; }                  // DonViTinh

        public decimal DonGiaUocTinh { get; set; }              // DonGiaUocTinh

        // ThanhTien (SQL dùng cột computed) – ở code có thể tính ở chỗ khác
        [NotMapped]
        public decimal ThanhTien => SoLuong * DonGiaUocTinh;

        [Column(TypeName = "NVARCHAR(500)")]
        public string? GhiChu { get; set; }                     // GhiChu
    }
}
