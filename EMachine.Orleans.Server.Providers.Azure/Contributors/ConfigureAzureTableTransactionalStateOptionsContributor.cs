using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Providers.Azure.Contributors;

internal sealed class ConfigureAzureTableTransactionalStateOptionsContributor : ConfigureOptionsContributorBase<AzureTableTransactionalStateOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Azure:TableTransactionalState";
}
