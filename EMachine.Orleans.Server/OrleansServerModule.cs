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
        context.Services.AddConfigureOptionsContributor<ConfigureActivationCountBasedPlacementOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureClusterMembershipOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureClusterOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureConnectionOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureConsistentRingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureCustomStorageLogConsistencyOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureDeploymentBasedQueueBalancerOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureDeploymentLoadPublisherOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureDevelopmentClusterMembershipOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureEndpointOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGatewayOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainCollectionOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainDirectoryOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainTypeOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainVersioningOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureHashRingStreamQueueMapperOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureLeaseBasedQueueBalancerOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureLoadSheddingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureMemoryGrainStorageOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSchedulingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSiloMessagingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSiloOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSimpleQueueCacheOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStaticGatewayListProviderOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamCacheEvictionOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamLifecycleOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamPubSubOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamPullingAgentOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureStreamStatisticOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureTransactionalStateOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureTypeManagementOptionsContributor>();
    }
}
