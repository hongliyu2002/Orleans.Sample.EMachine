using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class RedisPersistenceHealthChecksContributor : IHealthChecksContributor
{

    /// <inheritdoc />
    public void ConfigureHealthChecks(IHealthChecksBuilder builder, IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<RedisPersistenceOptions>();
        options.ConnectionStrings = context.Services.GetObject<ConnectionStrings>();
        foreach (var connectionStringName in options.ConnectionStringNames)
        {
            if (options.ConnectionStrings.TryGetValue(connectionStringName, out var connectionString))
            {
                builder.AddRedis(connectionString, $"RedisPersistence-{connectionStringName}", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
            }
        }
    }
}
