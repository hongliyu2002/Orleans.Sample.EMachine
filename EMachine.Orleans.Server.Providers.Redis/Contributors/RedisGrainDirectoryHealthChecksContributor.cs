﻿using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class RedisGrainDirectoryHealthChecksContributor : IHealthChecksContributor
{

    /// <inheritdoc />
    public void ConfigureHealthChecks(IHealthChecksBuilder builder, IServiceConfigurationContext context)
    {
        var options = context.Services.GetOptions<RedisGrainDirectoryOptions>();
        options.ConnectionStrings = context.Services.GetObject<ConnectionStrings>();
        if (options.ConnectionStrings.TryGetValue(options.ConnectionStringName, out var connectionString))
        {
            builder.AddRedis(connectionString, "RedisGrainDirectory", HealthStatus.Unhealthy, new[] { HealthCheckTags.Ready });
        }
    }
}
