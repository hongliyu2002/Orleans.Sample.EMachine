using Fluxera.Extensions.Hosting.Modules.Configuration;
using Orleans.Clustering.Redis;

namespace EMachine.Orleans.Server.Providers.Redis.Contributors;

internal sealed class ConfigureRedisClusteringOptionsContributor : ConfigureOptionsContributorBase<RedisClusteringOptions>
{
    /// <inheritdoc />
    public override string SectionName => "Orleans:Redis:Clustering";
}
