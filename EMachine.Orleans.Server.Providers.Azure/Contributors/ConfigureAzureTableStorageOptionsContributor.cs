using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Providers.Azure.Contributors;

internal sealed class ConfigureAzureTableStorageOptionsContributor : ConfigureOptionsContributorBase<AzureTableStorageOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Azure:TableStorage";
}
