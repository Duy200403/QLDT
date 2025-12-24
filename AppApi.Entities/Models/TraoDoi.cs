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
    [Table("TraoDoi")]
    public class TraoDoi : AuditEntity<Guid>
    {
        public TraoDoi()
        {
        }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string LoaiDoiTuong { get; set; } = null!;   // DEXUAT, HOSOTHAU, ...

        // Id của đối tượng tương ứng (DeXuatMuaSam.Id, HoSoThau.Id...)
        public Guid DoiTuongId { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? BuocMa { get; set; }                 // DX-02, B2, N1-B3...

        // FK -> Account (người gửi)
        public Guid NguoiGuiId { get; set; }

        [ForeignKey(nameof(NguoiGuiId))]
        public virtual Account NguoiGui { get; set; } = null!;

        // FK -> Account (người nhận đích danh, có thể null)
        public Guid? NguoiNhanId { get; set; }

        [ForeignKey(nameof(NguoiNhanId))]
        public virtual Account? NguoiNhan { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? RoleNguoiNhan { get; set; }          // nếu gửi theo role

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? NoiDung { get; set; }

        public DateTime ThoiGianGui { get; set; } = DateTime.Now;

        [Column(TypeName = "NVARCHAR(20)")]
        public string? TrangThai { get; set; }              // OPEN / RESOLVED

        public bool IsSystem { get; set; } = false;
    }
}
