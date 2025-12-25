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
    [Table("NhaThau")]
    public class NhaThau : AuditEntity<Guid>
    {
        public NhaThau()
        {
        }

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenNhaThau { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(50)")]
        public string? MaSoThue { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? DiaChi { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? LienHe { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? Email { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public string? DienThoai { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        public TrangThaiNhaThau TrangThai { get; set; } = TrangThaiNhaThau.HoatDong;
        public virtual ICollection<DanhGiaNhaThau> DanhGiaNhaThaus { get; set; } = new HashSet<DanhGiaNhaThau>();

        // Navigation 2 chiều sẽ được bổ sung ở các entity sau (HoSoDuThau, ThuMoiBaoGiaCT, ThuMoiThauCT, HopDong, DanhGiaNhaThau)
        // để tránh lỗi type chưa tồn tại, ở đây tạm thời không khai báo ICollection.
    }
}