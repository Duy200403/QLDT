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
    [Table("ThanhVienToChuyenGia")]
    public class ThanhVienToChuyenGia : AuditEntity<Guid>
    {
        public ThanhVienToChuyenGia()
        {
        }

        // FK -> ToChuyenGia
        public Guid ToChuyenGiaId { get; set; }

        [ForeignKey(nameof(ToChuyenGiaId))]
        public virtual ToChuyenGia ToChuyenGia { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string HoTen { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(255)")]
        public string? DonVi { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public string? VaiTro { get; set; }      // Tổ trưởng / Thành viên...
    }
}
