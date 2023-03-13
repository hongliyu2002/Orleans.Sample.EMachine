using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureSimpleQueueCacheOptionsContributor : ConfigureOptionsContributorBase<SimpleQueueCacheOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:SimpleQueueCache";
}
