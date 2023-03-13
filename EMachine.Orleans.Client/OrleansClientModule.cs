using EMachine.Orleans.Client.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Orleans.Client;

[PublicAPI]
[DependsOn<ConfigurationModule>]
public class OrleansClientModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureClientOptionsContributor>();

        // Configure core options
        context.Services.AddConfigureOptionsContributor<ConfigureClientMessagingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureClusterOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureConnectionOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGatewayOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStaticGatewayListProviderOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureTypeManagementOptionsContributor>();

        // Configure broadcast options. 
        context.Services.AddConfigureOptionsContributor<ConfigureBroadcastChannelOptionsContributor>();
    }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        var clientOptions = context.Services.GetOptions<ClientOptions>();
        context.Log("AddOrleansClient", services => services.AddOrleansClient(clientOptions));
    }
}
