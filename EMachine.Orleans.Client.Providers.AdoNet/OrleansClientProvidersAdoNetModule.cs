using EMachine.Orleans.Client.Providers.AdoNet.Contributors;
using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.DataManagement;
using JetBrains.Annotations;

namespace EMachine.Orleans.Client.Providers.AdoNet;

[PublicAPI]
[DependsOn<DataManagementModule>]
[DependsOn<ConfigurationModule>]
public class OrleansClientProvidersAdoNetModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetClusteringOptionsContributor>();
    }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        var connectionStrings = context.Services.GetObject<ConnectionStrings>();
        var clusteringOptions = context.Services.GetOptions<AdoNetClusteringOptions>();
        clusteringOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansAdoNetClustering", services => services.AddOrleansClientAdoNetClustering(clusteringOptions));
    }
}
