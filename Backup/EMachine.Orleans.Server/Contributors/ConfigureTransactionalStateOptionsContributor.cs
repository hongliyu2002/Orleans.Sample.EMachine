using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureTransactionalStateOptionsContributor : ConfigureOptionsContributorBase<TransactionalStateOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:TransactionalState";
}
