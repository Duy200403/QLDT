using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AppApi.Entities.Models.Base;

namespace AppApi.Entities.Models
{

    [Table("Test")]
    public class Test : AuditEntity<Guid>
    {
        public Test()
        {

        }
        [Column(TypeName = "NVARCHAR(250)")]
        public string TestName { get; set; }
        [Column(TypeName = "NVARCHAR(250)")]
        public string TestCode { get; set; }
    }
}