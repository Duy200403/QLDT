using AppApi.Entities.Models.Base;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AppApi.Entities.Models
{
    [Table("Log")]
    public class Log : EntityBase<Guid>
    {
        public Log()
        {

        }
        // nội dung log
        public string Message { get; set; }
        public string Parameter { get; set; }

        [Column(TypeName = "NVARCHAR(50)")]
        public LogLevelWebInfo LogLevel { get; set; }

        [Column(TypeName = "NVARCHAR(100)")]
        public DateTime? CreatedDate { get; set; }
    }
}