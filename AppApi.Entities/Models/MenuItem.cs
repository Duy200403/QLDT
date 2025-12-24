using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using AppApi.Entities.Models.Base;

namespace AppApi.Entities.Models
{

    [Table("MenuItem")]
    public class MenuItem : AuditEntity<Guid>
    {
        public MenuItem()
        {

        }
        public string Title { get; set; }
        public string Path { get; set; }
        public string Icon { get; set; }
        public int OrderNumber { get; set; }

        // có ParentId để EF theo convention, không cần tạo FK, 2 cái giống Parent là được
        public Guid? ParentId { get; set; }
        [JsonIgnore]
        public MenuItem Parent { get; set; }
        [JsonIgnore]
        public ICollection<MenuItem> Children { get; set; }
        [JsonIgnore]
        public ICollection<ApiRoleMapping> ApiRoleMappings { get; set; }
    }
}