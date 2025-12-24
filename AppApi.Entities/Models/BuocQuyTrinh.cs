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
    [Table("BuocQuyTrinh")]
    public class BuocQuyTrinh : AuditEntity<Guid>
    {
        public BuocQuyTrinh()
        {
        }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaBuoc { get; set; } = null!;      // B1, B2, N1-B1, ...

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenBuoc { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(50)")]
        public string? NhomQuyTrinh { get; set; }        // CHUNG_B1_B12 / NHANH1 / N2_NHOM1...

        public int ThuTu { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string RoleXuLyChinh { get; set; } = null!; // MaRole xử lý chính (ROLE_...)

        public bool BatBuoc { get; set; } = true;

        [Column(TypeName = "NVARCHAR(50)")]
        public string? LoaiForm { get; set; }            // TRINH_DUYET / NHAP_KT / ...

        // 1 bước -> N bản ghi tiến trình
        public virtual ICollection<TienTrinhHoSo> TienTrinhHoSos { get; set; } = new HashSet<TienTrinhHoSo>();
    }
}
