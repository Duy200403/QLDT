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
    [Table("DuToanChiTiet")]
    public class DuToanChiTiet : AuditEntity<Guid>
    {
        public DuToanChiTiet()
        {
        }

        // FK -> DuToanGoiThau
        public Guid DuToanGoiThauId { get; set; }

        [ForeignKey(nameof(DuToanGoiThauId))]
        public virtual DuToanGoiThau DuToanGoiThau { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenHangHoaDichVu { get; set; } = null!;

        public decimal SoLuong { get; set; }

        public decimal DonGiaDuToan { get; set; }

        // SQL dùng cột computed ThanhTien = SoLuong * DonGiaDuToan
        // Ở code-first ta để [NotMapped] để dùng cho hiển thị nếu cần
        [NotMapped]
        public decimal ThanhTien => SoLuong * DonGiaDuToan;

        [Column(TypeName = "NVARCHAR(255)")]
        public string? CanCu { get; set; }      // nhà thầu A, báo giá số...

        [Column(TypeName = "NVARCHAR(500)")]
        public string? GhiChu { get; set; }
    }
}