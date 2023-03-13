using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureConsistentRingOptionsContributor : ConfigureOptionsContributorBase<ConsistentRingOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:ConsistentRing";
}
