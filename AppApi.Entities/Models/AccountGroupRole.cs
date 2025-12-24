using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppApi.Entities.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace AppApi.Entities.Models
{
    [Table("AccountGroupRole")]
    public class AccountGroupRole : AuditEntity<Guid>
    {
        public AccountGroupRole()
        {

        }
        [Required, Column(TypeName = "NVARCHAR(250)")]
        public string Username { get; set; } = default!;   // liên kết logic sang AuthServer
        public string AccountId { get; set; }
        public string RoleId { get; set; }

        public Guid GroupId { get; set; }

        public Group Group { get; set; } = default!;
    }
}