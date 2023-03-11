using EMachine.Orleans.Server.Providers.Redis.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.DataManagement;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.Redis;

[PublicAPI]
[DependsOn<HealthChecksModule>]
[DependsOn<DataManagementModule>]
[DependsOn<OpenTelemetryModule>]
[DependsOn<ConfigurationModule>]
public class OrleansServerProvidersRedisModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureRedisClusteringOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureRedisGrainDirectoryOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureRedisReminderTableOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureRedisStorageOptionsContributor>();
    }
}
