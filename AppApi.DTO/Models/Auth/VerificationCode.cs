using System;

namespace AppApi.DTO.Models.Auth
{
  public class VerificationCode
  {
    public string Code { get; set; }
    public DateTime ExpiredAt { get; set; }
    public bool IsUsed { get; set; }
  }
}