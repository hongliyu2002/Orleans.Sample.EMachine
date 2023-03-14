using EMachine.Orleans.Server.Providers.InMemory.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.DataManagement;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.InMemory;

[PublicAPI]
[DependsOn<HealthChecksModule>]
[DependsOn<DataManagementModule>]
[DependsOn<OpenTelemetryModule>]
[DependsOn<ConfigurationModule>]
public class OrleansServerProvidersInMemoryModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureInMemoryClusteringOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureInMemoryPersistenceOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureInMemoryReminderOptionsContributor>();
    }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        var clusteringOptions = context.Services.GetOptions<InMemoryClusteringOptions>();
        context.Log("AddOrleansInMemoryClustering", services => services.AddOrleansInMemoryClustering(clusteringOptions));
        var reminderOptions = context.Services.GetOptions<InMemoryReminderOptions>();
        context.Log("AddOrleansInMemoryReminder", services => services.AddOrleansInMemoryReminder(reminderOptions));
        var persistenceOptions = context.Services.GetOptions<InMemoryPersistenceOptions>();
        context.Log("AddOrleansInMemoryPersistence", services => services.AddOrleansInMemoryPersistence(persistenceOptions));
    }
}
