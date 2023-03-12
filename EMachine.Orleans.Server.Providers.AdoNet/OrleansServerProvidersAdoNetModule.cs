using EMachine.Orleans.Server.Providers.AdoNet.Contributors;
using Fluxera.Extensions.DataManagement;
using Fluxera.Extensions.DependencyInjection;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using Fluxera.Extensions.Hosting.Modules.DataManagement;
using Fluxera.Extensions.Hosting.Modules.HealthChecks;
using Fluxera.Extensions.Hosting.Modules.OpenTelemetry;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.AdoNet;

[PublicAPI]
[DependsOn<HealthChecksModule>]
[DependsOn<DataManagementModule>]
[DependsOn<OpenTelemetryModule>]
[DependsOn<ConfigurationModule>]
public class OrleansServerProvidersAdoNetModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetClusteringOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetPersistenceOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAdoNetReminderOptionsContributor>();
        context.Services.AddHealthCheckContributor<AdoNetClusteringHealthChecksContributor>();
        context.Services.AddHealthCheckContributor<AdoNetReminderHealthChecksContributor>();
        context.Services.AddHealthCheckContributor<AdoNetPersistenceHealthChecksContributor>();
        context.Services.AddTracerProviderContributor<TracerProviderContributor>();
    }

    /// <inheritdoc />
    public override void ConfigureServices(IServiceConfigurationContext context)
    {
        var connectionStrings = context.Services.GetObject<ConnectionStrings>();
        var clusteringOptions = context.Services.GetOptions<AdoNetClusteringOptions>();
        clusteringOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansAdoNetClustering", services => services.AddOrleansAdoNetClustering(clusteringOptions));
        var reminderOptions = context.Services.GetOptions<AdoNetReminderOptions>();
        reminderOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansAdoNetReminder", services => services.AddOrleansAdoNetReminder(reminderOptions));
        var persistenceOptions = context.Services.GetOptions<AdoNetPersistenceOptions>();
        persistenceOptions.ConnectionStrings = connectionStrings;
        context.Log("AddOrleansAdoNetPersistence", services => services.AddOrleansAdoNetPersistence(persistenceOptions));
    }
}
