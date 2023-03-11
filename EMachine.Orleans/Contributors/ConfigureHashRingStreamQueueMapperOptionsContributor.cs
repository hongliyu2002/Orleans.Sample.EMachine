using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Contributors;

internal sealed class ConfigureHashRingStreamQueueMapperOptionsContributor : ConfigureOptionsContributorBase<HashRingStreamQueueMapperOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:HashRingStreamQueueMapper";
}
