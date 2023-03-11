using EMachine.Orleans.Server.Providers.Azure.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Orleans.Server.Providers.Azure;

[PublicAPI]
[DependsOn<ConfigurationModule>]
public class OrleansServerProvidersAzureModule : ConfigureServicesModule
{
    /// <inheritdoc />
    public override void PreConfigureServices(IServiceConfigurationContext context)
    {
        context.Services.AddConfigureOptionsContributor<ConfigureAzureBlobLeaseProviderOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAzureBlobStorageOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAzureQueueOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAzureTableGrainDirectoryOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAzureTableStorageOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAzureTableStreamCheckpointerOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureAzureTableTransactionalStateOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureEventDataGeneratorStreamOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureEventHubOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureEventHubReceiverOptionsContributor>();
        context.Services.AddConfigureOptionsContributor<ConfigureEventHubStreamCachePressureOptionsContributor>();
    }
}
