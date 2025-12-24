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
    [Table("ThongTinKyThuatChiTiet")]
    public class ThongTinKyThuatChiTiet : AuditEntity<Guid>
    {
        public ThongTinKyThuatChiTiet()
        {
        }

        // FK -> ThongTinKyThuat (ThongTinKyThuatId INT trong SQL)
        public Guid ThongTinKyThuatId { get; set; }

        [ForeignKey(nameof(ThongTinKyThuatId))]
        public virtual ThongTinKyThuat ThongTinKyThuat { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenHangHoaDichVu { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(100)")]
        public string? MaHang { get; set; }

        public decimal SoLuong { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? DonViTinh { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? YeuCauKyThuat { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string? GhiChu { get; set; }
    }
}