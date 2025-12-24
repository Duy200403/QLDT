using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("ThongTinKyThuat")]
    public class ThongTinKyThuat : AuditEntity<Guid>
    {
        public ThongTinKyThuat()
        {
        }

        // FK -> HoSoThau
        public Guid HoSoThauId { get; set; }

        [ForeignKey(nameof(HoSoThauId))]
        public virtual HoSoThau HoSoThau { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? MoTaChung { get; set; }

        // FK -> Account (người nhập)
        public Guid? NguoiNhapId { get; set; }

        [ForeignKey(nameof(NguoiNhapId))]
        public virtual Account? NguoiNhap { get; set; }

        public DateTime NgayNhap { get; set; } = DateTime.Now;

        // Lần sau tạo entity ThongTinKyThuatChiTiet, ta sẽ bổ sung navigation tại đó
        // để tránh lỗi thiếu type, ở đây không khai báo ICollection<ThongTinKyThuatChiTiet>.
    }
}