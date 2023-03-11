using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Clustering.AzureStorage;

namespace EMachine.Orleans.Server.Providers.Azure.Contributors;

internal sealed class ConfigureAzureStoragePolicyOptionsContributor : ConfigureOptionsContributorBase<AzureStoragePolicyOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Azure:StoragePolicy";
}
