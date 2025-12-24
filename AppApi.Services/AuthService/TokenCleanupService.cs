using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace AppApi.Services.AuthService
{
    public class TokenCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<TokenCleanupService> _logger;

        public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var tokenManager = scope.ServiceProvider.GetRequiredService<IOpenIddictTokenManager>();
                var authManager = scope.ServiceProvider
                                .GetRequiredService<IOpenIddictAuthorizationManager>();

                try
                {
                    // await tokenManager.PruneAsync(DateTimeOffset.UtcNow.AddMinutes(-1), stoppingToken);
                    // _logger.LogInformation("Expired tokens pruned at {Time}", DateTime.UtcNow);

                    // var threshold = DateTimeOffset.UtcNow;
                    var threshold = DateTimeOffset.UtcNow.AddDays(-1);
                    // Xoá token đã hết hạn
                    await tokenManager.PruneAsync(threshold, stoppingToken);
                    // Xoá authorization không còn liên kết với bất kỳ token nào, hoặc đã hết hạn
                    await authManager.PruneAsync(threshold, stoppingToken);
                    _logger.LogInformation("Pruned tokens & authorizations at {Time}", DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error pruning expired tokens.");
                }

                // Wait 24 hours before the next cleanup
                await Task.Delay(TimeSpan.FromMinutes(4), stoppingToken);
            }
        }
    }
}