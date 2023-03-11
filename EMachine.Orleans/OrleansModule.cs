using EMachine.Orleans.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Orleans;

[PublicAPI]
[DependsOn<ConfigurationModule>]
public class OrleansModule : ConfigureServicesModule
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
        context.Services.AddConfigureOptionsContributor<ConfigureGrainCollectionOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureGrainVersioningOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureLoadSheddingOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureSiloOptionsContributor>();
    }
}
