using System;

namespace AppApi.DTO.Models.Logger
{
    public class LogResponse
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public string ByAccount { get; set; }
        public string Parameter { get; set; }
        public string LogLevel { get; set; }
        public string CreatedDate { get; set; }
        
    }
}