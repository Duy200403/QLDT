using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("ThuMoiThau_ChiTiet")]
    public class ThuMoiThauChiTiet : AuditEntity<Guid>
    {
        public ThuMoiThauChiTiet()
        {
        }

        // FK -> ThuMoiThau (ThuMoiId trong SQL)
        public Guid ThuMoiId { get; set; }

        [ForeignKey(nameof(ThuMoiId))]
        public virtual ThuMoiThau ThuMoiThau { get; set; } = null!;

        // FK -> NhaThau
        public Guid NhaThauId { get; set; }

        [ForeignKey(nameof(NhaThauId))]
        public virtual NhaThau NhaThau { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(50)")]
        public string? HinhThucGui { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        public string? ThongTinLienHe { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        public string? TrangThai { get; set; }
    }
}