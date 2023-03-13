using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Configuration;

namespace EMachine.Orleans.Server.Contributors;

internal sealed class ConfigureStreamCacheEvictionOptionsContributor : ConfigureOptionsContributorBase<StreamCacheEvictionOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:StreamCacheEviction";
}
