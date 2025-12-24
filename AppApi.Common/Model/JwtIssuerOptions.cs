using AppApi.Common.Model.Interface;

namespace AppApi.Common.Model
{
  public class JwtIssuerOptions : IJwtIssuerOptions
  {
      public string Secret { get; set; }
      public string JwtIssuer { get; set; }
      public string JwtAudience { get; set; }
      public int JwtExpireDays { get; set; }
  }
}