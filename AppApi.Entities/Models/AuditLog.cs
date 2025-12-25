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
    [Table("AuditLog")]
    public class AuditLog : AuditEntity<Guid>
    {
        public AuditLog()
        {
        }
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string TableName { get; set; } = null!; // Tên bảng bị thay đổi

        [Required]
        [Column(TypeName = "NVARCHAR(20)")]
        public string Action { get; set; } = null!; // Các hành động: CREATE, UPDATE, DELETE

        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string RecordId { get; set; } = null!; // ID của bản ghi bị thay đổi

        public string? OldValues { get; set; } // Lưu giá trị cũ (định dạng JSON)
        public string? NewValues { get; set; } // Lưu giá trị mới (định dạng JSON)
    }
}
