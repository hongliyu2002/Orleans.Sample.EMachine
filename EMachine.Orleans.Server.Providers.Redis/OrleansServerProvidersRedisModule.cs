using EMachine.Orleans.Server.Providers.Redis.Contributors;
using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
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
        context.Services.AddHealthCheckContributor<RedisClusteringHealthChecksContributor>();
        context.Services.AddHealthCheckContributor<RedisGrainDirectoryHealthChecksContributor>();
        context.Services.AddHealthCheckContributor<RedisReminderTableHealthChecksContributor>();
        context.Services.AddHealthCheckContributor<RedisStorageHealthChecksContributor>();
        context.Services.AddTracerProviderContributor<TracerProviderContributor>();
    }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        var connectionStrings = context.Services.GetObject<ConnectionStrings>();
        var clusteringOptions = context.Services.GetOptions<RedisClusteringOptions>();
        clusteringOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansRedisClustering", services => services.AddOrleansRedisClustering(clusteringOptions));
        var grainDirectoryOptions = context.Services.GetOptions<RedisGrainDirectoryOptions>();
        grainDirectoryOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansRedisGrainDirectory", services => services.AddOrleansRedisGrainDirectory(grainDirectoryOptions));
        var reminderTableOptions = context.Services.GetOptions<RedisReminderTableOptions>();
        reminderTableOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansRedisReminder", services => services.AddOrleansRedisReminder(reminderTableOptions));
        var storageOptions = context.Services.GetOptions<RedisStorageOptions>();
        storageOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansRedisStorage", services => services.AddOrleansRedisStorage(storageOptions));
    }
}
