using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class RedisReminderTableHealthChecksContributor : IHealthChecksContributor
{

    /// <inheritdoc />
    public void ConfigureHealthChecks(IHealthChecksBuilder builder, IServiceConfigurationContext context)
    {
        var redisOptions = context.Services.GetOptions<RedisReminderTableOptions>();
        redisOptions.ConnectionStrings = context.Services.GetObject<ConnectionStrings>();
        if (redisOptions.ConnectionStrings.TryGetValue(redisOptions.ConnectionStringName, out var connectionString))
        {
            builder.AddRedis(connectionString, "RedisReminderTable", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
        }
    }
}
