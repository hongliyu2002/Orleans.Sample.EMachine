using EMachine.Orleans.Client.Providers.Redis.Contributors;
using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.DataManagement;
using JetBrains.Annotations;

namespace EMachine.Orleans.Client.Providers.Redis;

[PublicAPI]
[DependsOn<DataManagementModule>]
[DependsOn<ConfigurationModule>]
public class OrleansClientProvidersRedisModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureRedisClusteringOptionsContributor>();
    }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        var connectionStrings = context.Services.GetObject<ConnectionStrings>();
        var clusteringOptions = context.Services.GetOptions<RedisClusteringOptions>();
        clusteringOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansRedisClustering", services => services.AddOrleansRedisClustering(clusteringOptions));
    }
}
