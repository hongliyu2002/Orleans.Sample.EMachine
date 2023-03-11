using EMachine.Orleans.Providers.AdoNet.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Orleans.Providers.AdoNet;

[PublicAPI]
[DependsOn<ConfigurationModule>]
public class OrleansProvidersAdoNetModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetClusteringSiloOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetGrainStorageOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetReminderTableOptionsContributor>();
    }
}
