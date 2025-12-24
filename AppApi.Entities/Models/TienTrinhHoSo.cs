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
    [Table("TienTrinhHoSo")]
    public class TienTrinhHoSo : AuditEntity<Guid>
    {
        public TienTrinhHoSo()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        // FK -> BuocQuyTrinh
        public Guid BuocQuyTrinhId { get; set; }

        [ForeignKey(nameof(BuocQuyTrinhId))]
        public virtual BuocQuyTrinh BuocQuyTrinh { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string TrangThaiBuoc { get; set; } = "CHUA_THUC_HIEN";   // CHUA_THUC_HIEN / DANG_XU_LY / DA_DUYET / TU_CHOI

        // Đơn vị xử lý thực tế (FK -> DonVi)
        public Guid? DonViXuLyThucTeId { get; set; }

        [ForeignKey(nameof(DonViXuLyThucTeId))]
        public virtual DonVi? DonViXuLyThucTe { get; set; }

        // Người xử lý thực tế (FK -> Account)
        public Guid? NguoiXuLyId { get; set; }

        [ForeignKey(nameof(NguoiXuLyId))]
        public virtual Account? NguoiXuLy { get; set; }

        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThuc { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? YKien { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? GhiChu { get; set; }

        // 1 bước trong tiến trình -> có thể có nhiều file đính kèm
        public virtual ICollection<FileDinhKem> FileDinhKems { get; set; } = new HashSet<FileDinhKem>();

        // Nếu sau này có ThamDinh, có thể thêm navigation ở đó (đừng thêm ở đây để tránh phụ thuộc sớm).
    }
}
