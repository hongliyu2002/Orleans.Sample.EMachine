using EMachine.Orleans.Providers.Azure.Contributors;
using Fluxera.Extensions.Hosting;
using Fluxera.Extensions.Hosting.Modules;
using Fluxera.Extensions.Hosting.Modules.Configuration;
using JetBrains.Annotations;

namespace EMachine.Orleans.Providers.Azure;

[PublicAPI]
[DependsOn<ConfigurationModule>]
public class OrleansProvidersAzureModule : ConfigureServicesModule
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
    }
}
