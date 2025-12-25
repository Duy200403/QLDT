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
    [Table("DonVi")]
    public class DonVi : AuditEntity<Guid>
    {
        public DonVi()
        {
        }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaDonVi { get; set; } = null!;         // MaDonVi trong SQL

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenDonVi { get; set; } = null!;        // TenDonVi

        [Column(TypeName = "NVARCHAR(100)")]
        public string? LoaiDonVi { get; set; }               // LoaiDonVi: khoa / phòng / ban...

        [Column(TypeName = "NVARCHAR(20)")]
        public string? TrangThai { get; set; }        
        // ACTIVE / INACTIVE
        [Column(TypeName = "VARCHAR(500)")]
        public string? Path { get; set; }
        // Cây đơn vị
        public Guid? DonViChaId { get; set; }
        public int? ThuTu { get; set; }

        [ForeignKey(nameof(DonViChaId))]
        public virtual DonVi? DonViCha { get; set; }

        public virtual ICollection<DonVi> DonViCon { get; set; } = new HashSet<DonVi>();

        // 1 Đơn vị -> N Đề xuất
        public virtual ICollection<DeXuatMuaSam> DeXuatMuaSams { get; set; } = new HashSet<DeXuatMuaSam>();

        // 1 Đơn vị -> N Gói thầu kế hoạch (Đơn vị đề xuất chính)
        [InverseProperty(nameof(GoiThauKeHoach.DonViDeXuatChinh))]
        public virtual ICollection<GoiThauKeHoach> GoiThauKeHoach_DeXuatChinh { get; set; } = new HashSet<GoiThauKeHoach>();

        // 1 Đơn vị -> N Gói thầu kế hoạch (Đơn vị mua sắm)
        [InverseProperty(nameof(GoiThauKeHoach.DonViMuaSam))]
        public virtual ICollection<GoiThauKeHoach> GoiThauKeHoach_MuaSam { get; set; } = new HashSet<GoiThauKeHoach>();
    }
}