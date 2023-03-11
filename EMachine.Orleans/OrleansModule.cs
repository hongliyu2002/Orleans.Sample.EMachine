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
        context.Services.AddConfigureOptionsContributor<ConfigureClusterOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureEndpointOptionsContributor>();
    }
}
