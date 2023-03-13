using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureHashRingStreamQueueMapperOptionsContributor : ConfigureOptionsContributorBase<HashRingStreamQueueMapperOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Server:HashRingStreamQueueMapper";
}
