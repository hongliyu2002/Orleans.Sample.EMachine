using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Clustering.AzureStorage;

namespace EMachine.Orleans.Server.Providers.Azure.Contributors;

internal sealed class ConfigureAzureStorageOperationOptionsContributor : ConfigureOptionsContributorBase<AzureStorageOperationOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Azure:StorageOperation";
}
