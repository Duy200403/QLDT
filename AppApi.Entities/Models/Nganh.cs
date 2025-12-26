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
    [Table("Nganh")]
    public class Nganh : AuditEntity<Guid>
    {
        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string MaNganh { get; set; } = null!;

        [Required]
        [Column(TypeName = "NVARCHAR(255)")]
        public string TenNganh { get; set; } = null!;

        public virtual ICollection<VatTu> VatTus { get; set; } = new HashSet<VatTu>();
    }
}