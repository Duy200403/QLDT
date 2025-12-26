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
    [Table("Kho")]
    public class Kho : AuditEntity<Guid>
    {
        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaKho { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenKho { get; set; } = null!;

        [Column(TypeName = "NVARCHAR(500)")]
        public string? DiaChi { get; set; }

        public virtual ICollection<VatTu> VatTus { get; set; } = new HashSet<VatTu>();
    }
}
