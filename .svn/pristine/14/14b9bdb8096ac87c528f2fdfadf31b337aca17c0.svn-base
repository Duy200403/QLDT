using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppApi.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Entities.Models
{
    [Table("Group")]
    public class Group : AuditEntity<Guid>
    {
        public Group()
        {

        }
        [Required, Column(TypeName = "NVARCHAR(150)")]
        public string Name { get; set; } = default!;

        [Column(TypeName = "NVARCHAR(500)")]
        public string Description { get; set; }

        // Navigation đến các user thuộc nhóm
        public virtual ICollection<AccountGroupRole> AccountGroupRoles { get; set; }
    }
}