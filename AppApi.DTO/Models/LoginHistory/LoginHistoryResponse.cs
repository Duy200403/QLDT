using System;

namespace AppApi.DTO.Models.LoginHistory
{
    public class LoginHistoryResponse
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public DateTime LoginDate { get; set; }
        public Guid AccountId { get; set; }
    }
}