using EMachine.Orleans.Server.Providers.Redis.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.Redis;

[PublicAPI]
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
