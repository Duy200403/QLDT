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
    [Table("HoSoThau")]
    public class HoSoThau : AuditEntity<Guid>
    {
        public HoSoThau()
        {
        }

        // FK -> GoiThauKeHoach (có thể null)
        public Guid? GoiThauKeHoachId { get; set; }

        [ForeignKey(nameof(GoiThauKeHoachId))]
        public virtual GoiThauKeHoach? GoiThauKeHoach { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string SoHoSo { get; set; } = null!;

        public int Nam { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenGoiThau { get; set; } = null!;

        // Đơn vị đề xuất (FK -> DonVi)
        public Guid DonViDeXuatId { get; set; }

        [ForeignKey(nameof(DonViDeXuatId))]
        public virtual DonVi DonViDeXuat { get; set; } = null!;

        // Đơn vị mua sắm (FK -> DonVi)
        public Guid DonViMuaSamId { get; set; }

        [ForeignKey(nameof(DonViMuaSamId))]
        public virtual DonVi DonViMuaSam { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(100)")]
        public string? LoaiMuaSam { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? LinhVuc { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string? HinhThucLCNT { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? LoaiQuyTrinh { get; set; }    // NHANH1 / N2_NHOM1 / ...

        public decimal GiaTriDuToan { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string? NguonVon { get; set; }

        public DateTime? ThoiGianTu { get; set; }
        public DateTime? ThoiGianDen { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(30)")]
        public string TrangThaiTong { get; set; } = "DRAFT"; // DRAFT / IN_PROGRESS / ...

        [Column(TypeName = "NVARCHAR(100)")]
        public string? BuocHienTai { get; set; }

        // FK -> Account (người tạo hồ sơ)
        public Guid? NguoiTaoId { get; set; }

        [ForeignKey(nameof(NguoiTaoId))]
        public virtual Account? NguoiTao { get; set; }

        // Navigation: 1 HoSoThau -> N tiến trình, N file, N thông tin kỹ thuật
        public virtual ICollection<TienTrinhHoSo> TienTrinhHoSos { get; set; } = new HashSet<TienTrinhHoSo>();

        public virtual ICollection<FileDinhKem> FileDinhKems { get; set; } = new HashSet<FileDinhKem>();
        public virtual ICollection<ThongTinKyThuat> ThongTinKyThuats { get; set; } = new HashSet<ThongTinKyThuat>();
        public virtual ICollection<DanhGiaNhaThau> DanhGiaNhaThaus { get; set; } = new HashSet<DanhGiaNhaThau>();

    }
}
