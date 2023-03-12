using EMachine.Persistence.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.DataManagement;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace EMachine.Persistence;

[PublicAPI]
[DependsOn<DataManagementModule>]
[DependsOn<OpenTelemetryModule>]
[DependsOn<ConfigurationModule>]
public sealed class PersistenceModule : ConfigureApplicationModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigurePersistenceOptionsContributor>();
    }

    /// <inheritdoc />
    public override void PostConfigureServices(IServiceConfigurationContext context)
    {
        // Add database name provider.
        context.Log("AddDatabaseNameProvider", services =>
                                               {
                                                   services.TryAddTransient<IDatabaseNameProvider, DefaultDatabaseNameProvider>();
                                               });
        // Add database connection string provider.
        context.Log("AddDatabaseConnectionStringProvider", services =>
                                                           {
                                                               services.TryAddTransient<IDatabaseConnectionStringProvider, DefaultDatabaseConnectionStringProvider>();
                                                           });
    }
}
