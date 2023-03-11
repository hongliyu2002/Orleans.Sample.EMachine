using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Providers.Azure.Contributors;

internal sealed class ConfigureAzureBlobLeaseProviderOptionsContributor : ConfigureOptionsContributorBase<AzureBlobLeaseProviderOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Azure:BlobLeaseProvider";
}
