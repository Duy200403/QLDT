using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppApi.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Entities.Models
{
    [Table("Role")]
    [Index(nameof(Name), IsUnique = true)]
    public class Role : AuditEntity<Guid>
    {
        public Role()
        {

        }
        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Name { get; set; }

        [Column(TypeName = "NVARCHAR(250)")]
        public string Description { get; set; }

        // public string RoleCode { get; set; }
        public Guid? ParentId { get; set; }
        // public virtual Role Parent { get; set; }

        // public virtual ICollection<Role> Children { get; set; } = new List<Role>();

        public int OrderNumber { get; set; }
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}