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
    [Table("GoiThauKeHoach")]
    public class GoiThauKeHoach : AuditEntity<Guid>
    {
        public GoiThauKeHoach()
        {
        }

        public int NamKeHoach { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? MaGoi { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenGoiThau { get; set; } = null!;

        // Đơn vị đề xuất chính
        public Guid DonViDeXuatChinhId { get; set; }

        [ForeignKey(nameof(DonViDeXuatChinhId))]
        [InverseProperty(nameof(DonVi.GoiThauKeHoach_DeXuatChinh))]
        public virtual DonVi DonViDeXuatChinh { get; set; } = null!;

        // Đơn vị mua sắm
        public Guid DonViMuaSamId { get; set; }

        [ForeignKey(nameof(DonViMuaSamId))]
        [InverseProperty(nameof(DonVi.GoiThauKeHoach_MuaSam))]
        public virtual DonVi DonViMuaSam { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(100)")]
        public string? LoaiMuaSam { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? LinhVuc { get; set; }

        public decimal GiaTriDuKien { get; set; }

        [Column(TypeName = "NVARCHAR(200)")]
        public string? HinhThucLCNTDuKien { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? LoaiQuyTrinhDuKien { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string TrangThaiKeHoach { get; set; } = "DRAFT";

        public Guid? NguoiTaoId { get; set; }

        [ForeignKey(nameof(NguoiTaoId))]
        public virtual Account? NguoiTao { get; set; }

        public virtual ICollection<GoiThauDeXuat> GoiThauDeXuats { get; set; } = new HashSet<GoiThauDeXuat>();
    }
}