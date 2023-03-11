using EMachine.Orleans.Server.Providers.AdoNet.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.AdoNet;

[PublicAPI]
[DependsOn<ConfigurationModule>]
public class OrleansServerProvidersAdoNetModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetClusteringSiloOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetGrainStorageOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetReminderTableOptionsContributor>();
    }
}
