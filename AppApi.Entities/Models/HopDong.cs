using AppApi.Entities.Enums;
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
    [Table("HopDong")]
    public class HopDong : AuditEntity<Guid>
    {
        public HopDong()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        // FK -> NhaThau
        public Guid NhaThauId { get; set; }

        [ForeignKey(nameof(NhaThauId))]
        public virtual NhaThau NhaThau { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string SoHopDong { get; set; } = null!;

        public DateTime? NgayKy { get; set; }

        public decimal? GiaTriHopDong { get; set; }

        public DateTime? ThoiGianTu { get; set; }
        public DateTime? ThoiGianDen { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? BaoHanhThongTin { get; set; }

        [Column(TypeName = "NVARCHAR(30)")]
        public TrangThaiHopDong TrangThai { get; set; } = TrangThaiHopDong.MoiTao;

        // Navigation
        public virtual ICollection<ThanhToan> ThanhToans { get; set; } = new HashSet<ThanhToan>();
        public virtual ICollection<NghiemThu> NghiemThus { get; set; } = new HashSet<NghiemThu>();
    }
}