using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("ThanhToan")]
    public class ThanhToan : AuditEntity<Guid>
    {
        public ThanhToan()
        {
        }

        // FK -> HopDong
        public Guid HopDongId { get; set; }

        [ForeignKey(nameof(HopDongId))]
        public virtual HopDong HopDong { get; set; } = null!;

        public int DotThanhToan { get; set; }

        public DateTime? NgayThanhToan { get; set; }

        public decimal? GiaTri { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? TaiLieuChungTu { get; set; }

        [Column(TypeName = "NVARCHAR(500)")]
        public string? GhiChu { get; set; }
    }
}