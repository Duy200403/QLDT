using AppApi.Entities.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppApi.Entities.Models
{
    [Table("GoiThauDeXuat")]
    public class GoiThauDeXuat : AuditEntity<Guid>
    {
        public GoiThauDeXuat()
        {
        }

        // FK -> GoiThauKeHoach (GoiThauId INT trong SQL)
        public Guid GoiThauKeHoachId { get; set; }

        [ForeignKey(nameof(GoiThauKeHoachId))]
        public virtual GoiThauKeHoach GoiThauKeHoach { get; set; } = null!;

        // FK -> DeXuatMuaSam (DeXuatId INT trong SQL)
        public Guid DeXuatId { get; set; }

        [ForeignKey(nameof(DeXuatId))]
        public virtual DeXuatMuaSam DeXuat { get; set; } = null!;
    }
}
