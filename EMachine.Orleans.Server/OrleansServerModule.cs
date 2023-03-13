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
        
        context.Services.AddConfigureOptionsContributor<ConfigureClusterOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureConnectionOptionsContributor>();
    }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        context.Log("AddOrleansServer", services => services.AddOrleansServer());
    }
}
