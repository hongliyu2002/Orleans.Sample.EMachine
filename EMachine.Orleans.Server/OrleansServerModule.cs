using EMachine.Orleans.Server.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server;

[PublicAPI]
[DependsOn<ConfigurationModule>]
public class OrleansServerModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureServerOptionsContributor>();

        // Configure core options
        context.Services.AddConfigureOptionsContributor<ConfigureClusterOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureConnectionOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainTypeOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainVersioningOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureLoadSheddingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureTypeManagementOptionsContributor>();

        // Configure runtime options. 
        context.Services.AddConfigureOptionsContributor<ConfigureActivationCountBasedPlacementOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureConsistentRingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureDeploymentLoadPublisherOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureDevelopmentClusterMembershipOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureEndpointOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainCollectionOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainDirectoryOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSchedulingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSiloMessagingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSiloOptionsContributor>();

        // Configure stream options. 
        context.Services.AddConfigureOptionsContributor<ConfigureDeploymentBasedQueueBalancerOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureHashRingStreamQueueMapperOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureLeaseBasedQueueBalancerOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSimpleQueueCacheOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamCacheEvictionOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamLifecycleOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamPubSubOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamPullingAgentOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamStatisticOptionsContributor>();

        // Configure broadcast options. 
        context.Services.AddConfigureOptionsContributor<ConfigureBroadcastChannelOptionsContributor>();
    }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        var serverOptions = context.Services.GetOptions<ServerOptions>();
        context.Log("AddOrleansServer", services => services.AddOrleansServer(serverOptions));
    }
}
