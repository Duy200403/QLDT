namespace AppApi.DTO.Models.OpeniddictRegistration
{
    public class RegisterAppWithScopeRequest
    {
        public string ClientAppId { get; set; }
        public string ClientAppDisplayName { get; set; }
        // public string ClientType { get; set; }
        // public List<string> RedirectUris { get; set; } = new();
        // public List<string> PostLogoutRedirectUris { get; set; } = new();

        public string ClientDomain { get; set; }

        public string ApiAppId { get; set; }
        public string ApiAppSecret { get; set; }
        public string ApiAppDisplayName { get; set; }

        public bool IsClientApp { get; set; } = false;

        public CustomScopeDto CustomScope { get; set; } = new(); // Custom scopes
    }

    public class CustomScopeDto
    {
        public string ScopeName { get; set; } // e.g., "report.read"
        public string ScopeDisplayName { get; set; } // e.g., "Xem Báo Cáo"
        // public List<string> Resources { get; set; } = new(); // e.g., ["WebApiId1992"]
    }
}