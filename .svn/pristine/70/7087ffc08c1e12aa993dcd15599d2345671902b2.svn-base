using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppApi.Entities.Models.Base;
using Newtonsoft.Json;

namespace AppApi.Entities.Models
{

    [Table("ApiRoleMapping")]
    public class ApiRoleMapping : AuditEntity<Guid>
    {
        public ApiRoleMapping()
        {

        }
        public string Controller { get; set; }
        public string Action { get; set; }
        // Cột thực trong DB
        public string AllowedRoles { get; set; } // "admin,doctor"

        // ✅ Cột không ánh xạ, xử lý dưới dạng List
        // [NotMapped]
        // public List<AllowedRole> LstAllowedRoles
        // {
        //     get => [.. (AllowedRoles ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
        //     set => AllowedRoles = string.Join(",", value ?? new List<string>());
        // }

         // EF ignores this, but JSON‐serializes it in your API
        [NotMapped]
        public List<AllowedRole> LstAllowedRoles
        {
            get
            {
                if (string.IsNullOrWhiteSpace(AllowedRoles))
                    return new List<AllowedRole>();

                return JsonConvert.DeserializeObject<List<AllowedRole>>(AllowedRoles)
                       ?? new List<AllowedRole>();
            }
            set
            {
                AllowedRoles = JsonConvert.SerializeObject(
                    value ?? new List<AllowedRole>(),
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    }
                );
            }
        }
        
        [JsonIgnore]
        public ICollection<MenuItem> MenuItems { get; set; }
    }
}